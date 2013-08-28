using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentResponseFactory : ResponseFactoryBase
    {
        public DocumentResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual DocumentResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<DocumentResponse>(response, OnSuccessfulDocumentResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulDocumentResponseContentMaterializer(HttpResponseMessage response, DocumentResponse result)
        {
            using (var content = response.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    ResponseMaterializer.PopulateDocumentHeaderResponse(result, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, result);
                    AssignMissingRevFromRequestHeaders(response, result);
                }

                content.Position = 0;
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    result.Content = reader.ReadToEnd();
                }
            }
        }
    }
}