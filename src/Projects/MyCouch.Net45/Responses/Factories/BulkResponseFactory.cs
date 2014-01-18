using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class BulkResponseFactory : ResponseFactoryBase
    {
        public BulkResponseFactory(ISerializer serializer)
            : base(serializer)
        {
        }

        public virtual BulkResponse Create(HttpResponseMessage httpResponse)
        {
            return Materialize(new BulkResponse(), httpResponse, OnSuccessfulResponse, OnFailedResponse);
        }

        protected async virtual void OnSuccessfulResponse(BulkResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                response.Rows = Serializer.Deserialize<BulkResponse.Row[]>(content);
            }
        }
    }
}