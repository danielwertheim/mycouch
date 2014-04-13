using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Responses.Factories.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class AttachmentResponseFactory : ResponseFactoryBase<AttachmentResponse>
    {
        protected readonly AttachmentResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public AttachmentResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new AttachmentResponseMaterializer();
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override AttachmentResponse CreateResponseInstance()
        {
            return new AttachmentResponse();
        }

        protected override void OnMaterializationOfSuccessfulResponseProperties(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void OnMaterializationOfFailedResponseProperties(AttachmentResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}