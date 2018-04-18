using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Factories
{
    public class FindResponseFactory : ResponseFactoryBase
    {
        protected readonly FindResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public FindResponseFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            SuccessfulResponseMaterializer = new FindResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<FindResponse> CreateAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<FindResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }

        public virtual async Task<FindResponse<TIncludedDoc>> CreateAsync<TIncludedDoc>(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<FindResponse<TIncludedDoc>>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}
