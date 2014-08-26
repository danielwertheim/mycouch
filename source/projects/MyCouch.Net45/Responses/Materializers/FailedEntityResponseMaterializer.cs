using System.Net.Http;
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
            Ensure.That(serializer, "serializer").IsNotNull();

            _failedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual void Materialize<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            _failedResponseMaterializer.Materialize(response, httpResponse);

            SetMissingIdFromRequestUri(response, httpResponse);
        }

        protected virtual void SetMissingIdFromRequestUri<T>(EntityResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            if (string.IsNullOrWhiteSpace(response.Id))
                response.Id = httpResponse.RequestMessage.ExtractIdFromUri(false);
        }
    }
}