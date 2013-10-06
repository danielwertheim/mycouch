using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ChangesResponseFactory : ResponseFactoryBase
    {
        public ChangesResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration) {}

        public virtual ChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ChangesResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(ChangesResponse response, HttpResponseMessage httpResponse)
        {
        }

        protected virtual void OnSuccessfulResponse<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
        }
    }
}