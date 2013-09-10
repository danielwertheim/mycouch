using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DatabaseResponseFactory : ResponseFactoryBase
    {
        public DatabaseResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) { }

        public virtual DatabaseResponse Create(HttpResponseMessage response)
        {
            return BuildResponse(new DatabaseResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, DatabaseResponse result) { }
    }
}