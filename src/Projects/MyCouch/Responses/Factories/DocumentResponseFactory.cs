using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentResponseFactory : DocumentHeaderResponseFactoryBase
    {
        public DocumentResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DocumentResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<DocumentResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, DocumentResponse result)
        {
            using (var content = response.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(response.RequestMessage))
                    PopulateDocumentHeaderResponse(result, content);
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