using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ContinuousChangesResponseFactory : ResponseFactoryBase
    {
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ContinuousChangesResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<ContinuousChangesResponse> CreateAsync(HttpResponseMessage httpResponse)
        {
            Ensure.That(httpResponse, "httpResponse").IsNotNull();

            return await MaterializeAsync<ContinuousChangesResponse>(
                httpResponse,
                null,
                FailedResponseMaterializer.MaterializeAsync).ForAwait();
        }
    }
}