using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        public ViewQueryResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual ViewQueryResponse<T> Create<T>(HttpResponseMessage response) where T : class
        {
            return CreateResponse<ViewQueryResponse<T>>(response, OnSuccessfulViewQueryResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulViewQueryResponseContentMaterializer<T>(HttpResponseMessage response, ViewQueryResponse<T> result) where T : class
        {
            using(var content = response.Content.ReadAsStream())
                ResponseMaterializer.PopulateViewQueryResponse(result, content);
        }
    }
}