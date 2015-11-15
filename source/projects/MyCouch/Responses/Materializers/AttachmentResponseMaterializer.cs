using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Materializers
{
    public class AttachmentResponseMaterializer
    {
        public virtual async Task MaterializeAsync(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            SetMissingIdFromRequestUri(response, httpResponse);
            SetMissingRevFromRequestHeaders(response, httpResponse);
            SetMissingNameFromRequestUri(response, httpResponse);
            await SetContentAsync(response, httpResponse).ForAwait();
        }

        protected virtual void SetMissingIdFromRequestUri(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(true);
        }

        protected virtual void SetMissingNameFromRequestUri(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Name))
                response.Name = httpResponse.RequestMessage.ExtractAttachmentNameFromUri();
        }

        protected virtual void SetMissingRevFromRequestHeaders(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Rev))
                response.Rev = httpResponse.Headers.GetETag();
        }

        protected virtual async Task SetContentAsync(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            response.Content = await httpResponse.Content.ReadAsByteArrayAsync().ForAwait();
        }
    }
}