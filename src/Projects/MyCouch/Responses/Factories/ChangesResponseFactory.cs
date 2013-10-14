using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ChangesResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        public ChangesResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = new DefaultSerializer(SerializationConfiguration);
        }

        public virtual ChangesResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ChangesResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ChangesResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(ChangesResponse<T> response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}