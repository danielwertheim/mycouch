using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class JsonViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ViewQueryResponseMaterializer ResponseMaterializer;

        public JsonViewQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new ViewQueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual JsonViewQueryResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<JsonViewQueryResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(HttpResponseMessage response, ViewQueryResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}