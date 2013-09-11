using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase
    {
        public DocumentHeaderResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new DocumentHeaderResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            if (httpResponse.RequestMessage.Method == HttpMethod.Head)
            {
                AssignMissingIdFromRequestUri(response, httpResponse);
                AssignMissingRevFromRequestHeaders(response, httpResponse);

                return;
            }

            using (var content = httpResponse.Content.ReadAsStream())
                AssignDocumentHeaderFromResponseStream(response, content);
        }

        protected virtual void OnFailedResponse(DocumentHeaderResponse response, HttpResponseMessage httpResponse)
        {
            base.OnFailedResponse(response, httpResponse);

            AssignMissingIdFromRequestUri(response, httpResponse);
        }
    }
}