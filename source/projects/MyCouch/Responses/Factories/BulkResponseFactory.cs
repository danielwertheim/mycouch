using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class BulkResponseFactory : ResponseFactoryBase
    {
        protected readonly BulkResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public BulkResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new BulkResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<BulkResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return await MaterializeAsync<BulkResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }
    }
}