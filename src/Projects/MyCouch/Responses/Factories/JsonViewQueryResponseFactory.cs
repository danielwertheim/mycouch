using System.Net.Http;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class JsonViewQueryResponseFactory : QueryResponseFactoryBase
    {
        public JsonViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
        }

        public virtual JsonViewQueryResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new JsonViewQueryResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }
    }
}