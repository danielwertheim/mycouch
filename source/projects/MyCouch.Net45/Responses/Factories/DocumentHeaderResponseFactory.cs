using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase<DocumentHeaderResponse>
    {
        protected readonly DocumentHeaderResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedDocumentHeaderResponseMaterializer FailedResponseMaterializer;

        public DocumentHeaderResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new DocumentHeaderResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedDocumentHeaderResponseMaterializer(serializer);
        }

        protected override DocumentHeaderResponse CreateResponseInstance()
        {
            return new DocumentHeaderResponse();
        }

        protected override void OnMaterializationOfSuccessfulResponseProperties(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void OnMaterializationOfFailedResponseProperties(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}