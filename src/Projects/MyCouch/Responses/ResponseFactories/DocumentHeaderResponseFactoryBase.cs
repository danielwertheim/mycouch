using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.ResponseFactories
{
    public abstract class DocumentHeaderResponseFactoryBase : ResponseFactoryBase
    {
        protected DocumentHeaderResponseFactoryBase(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        protected virtual void OnSuccessfulDocumentHeaderResponseContentMaterializer(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (response.RequestMessage.Method == HttpMethod.Head)
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                return;
            }

            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.PopulateDocumentHeaderResponse(result, content);
        }

        protected virtual void OnFailedDocumentHeaderResponseContentMaterializer(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            OnFailedResponseContentMaterializer(response, result);

            AssignMissingIdFromRequestUri(response, result);
        }
    }
}