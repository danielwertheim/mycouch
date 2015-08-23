using System.Net.Http;
using EnsureThat;
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

        public virtual void Materialize(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            _failedResponseMaterializer.Materialize(response, httpResponse);

            SetMissingIdFromRequestUri(response, httpResponse);
        }

        protected virtual void SetMissingIdFromRequestUri(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (string.IsNullOrWhiteSpace(response.Id) && httpResponse.RequestMessage.Method != HttpMethod.Post)
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(false);
        }
    }
}