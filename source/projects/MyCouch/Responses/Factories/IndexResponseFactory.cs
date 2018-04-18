using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Extensions;

namespace MyCouch.Responses.Factories
{
    public class IndexResponseFactory : ResponseFactoryBase
    {
        protected readonly SimpleDeserializingResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;
        public IndexResponseFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            SuccessfulResponseMaterializer = new SimpleDeserializingResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<IndexResponse> CreateAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<IndexResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}
