using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentResponseFactory : ResponseFactoryBase
    {
        public DocumentResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DocumentResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new DocumentResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                if (ContentShouldHaveIdAndRev(httpResponse.RequestMessage))
                    AssignDocumentHeaderFromResponseStream(response, content);
                else
                {
                    AssignMissingIdFromRequestUri(response, httpResponse);
                    AssignMissingRevFromRequestHeaders(response, httpResponse);
                }

                content.Position = 0;
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    response.Content = reader.ReadToEnd();
                }
            }
        }
    }
}