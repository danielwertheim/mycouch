using System.Net.Http;

namespace MyCouch.ResponseFactories
{
    public class DocumentHeaderResponseFactory : DocumentHeaderResponseFactoryBase
    {
        public DocumentHeaderResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<DocumentHeaderResponse>(response, OnSuccessfulDocumentHeaderResponseContentMaterializer, OnFailedDocumentHeaderResponseContentMaterializer);
        }
    }
}