using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class FailedEntityResponseMaterializer
    {
        private readonly FailedResponseMaterializer _failedResponseMaterializer;

        public FailedEntityResponseMaterializer(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            _failedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task MaterializeAsync<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            await _failedResponseMaterializer.MaterializeAsync(response, httpResponse).ForAwait();

            SetMissingIdFromRequestUri(response, httpResponse);
        }

        protected virtual void SetMissingIdFromRequestUri<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(false);
        }
    }
}