using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase
    {
        protected readonly AttachmentResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public AttachmentResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new AttachmentResponseMaterializer();
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual AttachmentResponse Create(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return Materialize<AttachmentResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}