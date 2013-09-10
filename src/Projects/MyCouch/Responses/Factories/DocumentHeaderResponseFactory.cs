using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : ResponseFactoryBase
    {
        public DocumentHeaderResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage response)
        {
            return BuildResponse(new DocumentHeaderResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            if (response.RequestMessage.Method == HttpMethod.Head)
            {
                AssignMissingIdFromRequestUri(response, result);
                AssignMissingRevFromRequestHeaders(response, result);

                return;
            }

            using (var content = response.Content.ReadAsStream())
                PopulateDocumentHeaderResponse(result, content);
        }

        protected virtual void OnFailedResponse(HttpResponseMessage response, DocumentHeaderResponse result)
        {
            base.OnFailedResponse(response, result);

            AssignMissingIdFromRequestUri(response, result);
        }
    }
}