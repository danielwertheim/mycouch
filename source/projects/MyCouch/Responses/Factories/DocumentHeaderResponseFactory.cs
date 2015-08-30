using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase
    {
        protected readonly DocumentHeaderResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedDocumentHeaderResponseMaterializer FailedResponseMaterializer;

        public DocumentHeaderResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new DocumentHeaderResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedDocumentHeaderResponseMaterializer(serializer);
        }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize<DocumentHeaderResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.Materialize,
                FailedResponseMaterializer.Materialize);
        }
    }
}