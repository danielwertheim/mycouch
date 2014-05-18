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
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                if (response.RequestMethod == HttpMethod.Get)
                {
                    response.Content = content.ReadAsString();
                    Serializer.Populate(response, response.Content);
                }
                else
                    Serializer.Populate(response, content);

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
    }
}