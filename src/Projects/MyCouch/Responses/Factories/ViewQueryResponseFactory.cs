using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        public ViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = new DefaultSerializer(SerializationConfiguration);
        }

        public virtual ViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage httpResponse)
        {
            return Materialize(new ViewQueryResponse<T>(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(ViewQueryResponse<T> response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                Serializer.Populate(response, content);
            }
        }
    }
}