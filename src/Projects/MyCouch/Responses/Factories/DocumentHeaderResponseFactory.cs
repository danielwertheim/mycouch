using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentHeaderResponseFactory : DocumentHeaderResponseFactoryBase
    {
        public DocumentHeaderResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DocumentHeaderResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<DocumentHeaderResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}