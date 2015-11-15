using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Materializers
{
    public class FailedDocumentHeaderResponseMaterializer
    {
        private readonly FailedResponseMaterializer _failedResponseMaterializer;

        public FailedDocumentHeaderResponseMaterializer(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            _failedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task MaterializeAsync(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            await _failedResponseMaterializer.MaterializeAsync(response, httpResponse).ForAwait();

            SetMissingIdFromRequestUri(response, httpResponse);
        }

        protected virtual void SetMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id) && httpResponse.RequestMessage.Method != HttpMethod.Post)
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(false);
        }
    }
}