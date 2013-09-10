using System.Net.Http;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonIndexQueryResponseFactory : QueryResponseFactoryBase
    {
        public JsonIndexQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual JsonIndexQueryResponse Create(HttpResponseMessage response)
        {
            return BuildResponse(new JsonIndexQueryResponse(), response, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}