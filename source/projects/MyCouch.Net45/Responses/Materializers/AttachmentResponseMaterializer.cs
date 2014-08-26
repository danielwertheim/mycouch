using System.Linq;
using System.Net.Http;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class AttachmentResponseMaterializer
    {
        public virtual void Materialize(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            SetMissingIdFromRequestUri(response, httpResponse);
            SetMissingRevFromRequestHeaders(response, httpResponse);
            SetMissingNameFromRequestUri(response, httpResponse);
            SetContent(response, httpResponse);
        }

        protected virtual void SetMissingIdFromRequestUri(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(true);
        }

        protected virtual void SetMissingNameFromRequestUri(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Name))
                response.Name = httpResponse.RequestMessage.RequestUri.Segments.LastOrDefault();
        }

        protected virtual void SetMissingRevFromRequestHeaders(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }

        protected virtual async void SetContent(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            response.Content = await httpResponse.Content.ReadAsByteArrayAsync().ForAwait();
        }
    }
}