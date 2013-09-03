using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonIndexQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly QueryResponseMaterializer ResponseMaterializer;

        public JsonIndexQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new QueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual JsonIndexQueryResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<JsonIndexQueryResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse(HttpResponseMessage response, JsonIndexQueryResponse result)
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}