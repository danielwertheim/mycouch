using System;
using System.Net.Http;
using System.Net.Http.Headers;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class DocumentResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public DocumentResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            if(response.RequestMethod != HttpMethod.Get)
                throw new ArgumentException(GetType().Name + " only supports materializing GET responses for raw documents.");

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                response.Content = content.ReadAsString();

                content.Position = 0;
                var t = Serializer.Deserialize<Temp>(content);
                response.Id = t._id;
                response.Rev = t._rev;

                SetMissingIdFromRequestUri(response, httpResponse.RequestMessage);
                SetMissingRevFromRequestHeaders(response, httpResponse.Headers);
            }
        }

        protected virtual void SetMissingIdFromRequestUri(DocumentResponse response, HttpRequestMessage request)
        {
            if (string.IsNullOrWhiteSpace(response.Id) && request.Method != HttpMethod.Post)
                response.Id = request.GetUriSegmentByRightOffset();
        }

        protected virtual void SetMissingRevFromRequestHeaders(DocumentResponse response, HttpResponseHeaders responseHeaders)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = responseHeaders.GetETag();
        }

        private class Temp
        {
            public string _id { get; set; }
            public string _rev { get; set; }
        }
    }
}