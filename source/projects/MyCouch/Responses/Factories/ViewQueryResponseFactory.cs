using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class ViewQueryResponseFactory : ResponseFactoryBase
    {
        protected readonly ViewQueryResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public ViewQueryResponseFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            SuccessfulResponseMaterializer = new ViewQueryResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        public virtual async Task<ViewQueryResponse> CreateAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<ViewQueryResponse>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }

        public virtual async Task<ViewQueryResponse<TValue>> CreateAsync<TValue>(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<ViewQueryResponse<TValue>>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }

        public virtual async Task<ViewQueryResponse<TValue, TIncludedDoc>> CreateAsync<TValue, TIncludedDoc>(HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            return await MaterializeAsync<ViewQueryResponse<TValue, TIncludedDoc>>(
                httpResponse,
                SuccessfulResponseMaterializer.MaterializeAsync,
                FailedResponseMaterializer.MaterializeAsync, cancellationToken).ForAwait();
        }
    }
}