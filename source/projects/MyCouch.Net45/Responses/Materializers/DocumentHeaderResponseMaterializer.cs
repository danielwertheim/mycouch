using System.IO;
using System.Net.Http;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class DocumentHeaderResponseMaterializer
    {
        protected readonly ISerializer Serializer;

        public DocumentHeaderResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual async void Materialize(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (httpResponse.RequestMessage.Method == HttpMethod.Head)
            {
                SetMissingIdFromRequestUri(response, httpResponse);
                SetMissingRevFromRequestHeaders(response, httpResponse);

                return;
            }

            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                SetDocumentHeaderFromResponseStream(response, content);
        }

        protected virtual void SetMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id) && httpResponse.RequestMessage.Method != HttpMethod.Post)
                response.Id = httpResponse.RequestMessage.GetUriSegmentByRightOffset();
        }

        protected virtual void SetMissingRevFromRequestHeaders(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }

        protected virtual void SetDocumentHeaderFromResponseStream(DocumentHeaderResponse response, Stream content)
        {
            Serializer.Populate(response, content);
        }
    }
}