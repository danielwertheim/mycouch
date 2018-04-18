using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Factories
{
    public class IndexListResponseFactory : ResponseFactoryBase
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public IndexListResponseFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<IndexListResponse> CreateAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<IndexListResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}
