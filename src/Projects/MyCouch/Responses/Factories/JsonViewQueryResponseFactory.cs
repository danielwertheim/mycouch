using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class JsonViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly QueryResponseMaterializer ResponseMaterializer;

        public JsonViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new QueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual JsonViewQueryResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<JsonViewQueryResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, JsonViewQueryResponse result)
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}