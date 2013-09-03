using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonIndexQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly IQyeryResponseMaterializer ResponseMaterializer;

        public JsonIndexQueryResponseFactory(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            ResponseMaterializer = new ViewQueryResponseMaterializer(SerializationConfiguration);
        }

        public virtual JsonIndexQueryResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<JsonIndexQueryResponse>(response, OnSuccessfulResponse, OnFailedResponse);
        }

        protected virtual void OnSuccessfulResponse<T>(HttpResponseMessage response, IndexQueryResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.Populate(result, content);
        }
    }
}