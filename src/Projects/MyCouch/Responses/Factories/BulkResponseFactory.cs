using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class BulkResponseFactory : ResponseFactoryBase
    {
        protected readonly ISerializer Serializer;

        public BulkResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = new DefaultSerializer(SerializationConfiguration);
        }

        public virtual BulkResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new BulkResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(BulkResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = httpResponse.Content.ReadAsStream())
            {
                response.Rows = Serializer.Deserialize<BulkResponse.Row[]>(content);
            }
        }
    }
}