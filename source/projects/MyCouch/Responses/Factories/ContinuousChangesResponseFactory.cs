using System.Net.Http;
using System.Threading;
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
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<ContinuousChangesResponse> CreateAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(httpResponse, nameof(httpResponse));

            return await MaterializeAsync<ContinuousChangesResponse>(
                httpResponse,
                null,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}