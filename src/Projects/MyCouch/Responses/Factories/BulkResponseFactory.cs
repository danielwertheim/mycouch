using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class BulkResponseFactory : ResponseFactoryBase
    {
        public BulkResponseFactory(IResponseMaterializer responseMaterializer) : base(responseMaterializer) { }

        public virtual BulkResponse Create(HttpResponseMessage response)
        {
            return CreateResponse<BulkResponse>(response, OnSuccessfulBulkResponseContentMaterializer, OnFailedResponseContentMaterializer);
        }

        protected virtual void OnSuccessfulBulkResponseContentMaterializer(HttpResponseMessage response, BulkResponse result)
        {
            using (var content = response.Content.ReadAsStream())
                ResponseMaterializer.PopulateBulkResponse(result, content);
        }
    }
}