using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Responses.Factories
{
    public class JsonIndexQueryResponseFactory : ResponseFactoryBase
    {
        public JsonIndexQueryResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual JsonIndexQueryResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<JsonIndexQueryResponse>(response, OnSuccessfulViewQueryResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulViewQueryResponseContentMaterializer<T>(HttpResponseMessage response, IndexQueryResponse<T> result) where T : class
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.PopulateViewQueryResponse(result, content);
        }
    }
}