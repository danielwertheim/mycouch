using System.IO;
using System.Net.Http;
using System.Text;
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
                    PopulateDocumentHeaderFromResponseStream(response, content);
                else
                {
                    PopulateMissingIdFromRequestUri(response, httpResponse);
                    PopulateMissingRevFromRequestHeaders(response, httpResponse);
                }

                content.Position = 0;

                var sb = new StringBuilder();
                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    while (!reader.EndOfStream)
                    {
                        sb.Append(reader.ReadLine());
                    }
                }
                response.Content = sb.ToString();
                sb.Clear();
            }
        }
    }
}