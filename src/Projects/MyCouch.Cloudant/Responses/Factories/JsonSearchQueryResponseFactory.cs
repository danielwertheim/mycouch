using System.Net.Http;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonSearchQueryResponseFactory : QueryResponseFactoryBase
    {
        public JsonSearchQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual JsonSearchQueryResponse Create(HttpResponseMessage response)
        {
            return Materialize(new JsonSearchQueryResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}