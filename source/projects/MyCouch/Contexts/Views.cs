using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase<IDbConnection>, IViews
    {
        protected QueryViewHttpRequestFactory QueryViewHttpRequestFactory { get; set; }
        protected ViewQueryResponseFactory ViewQueryResponseFactory { get; set; }
        protected RawResponseFactory RawResponseFactory { get; set; }

        public Views(IDbConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            QueryViewHttpRequestFactory = new QueryViewHttpRequestFactory(serializer);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(serializer);
            RawResponseFactory = new RawResponseFactory(serializer);
        }

        public virtual async Task<RawResponse> QueryRawAsync(QueryViewRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = QueryViewHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await RawResponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<ViewQueryResponse> QueryAsync(QueryViewRequest request, CancellationToken cancellationToken = default)
        {
            EnsureThatNoListFunctionIsUsed(request);

            var httpRequest = QueryViewHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await ViewQueryResponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<ViewQueryResponse<TValue>> QueryAsync<TValue>(QueryViewRequest request, CancellationToken cancellationToken = default)
        {
            EnsureThatNoListFunctionIsUsed(request);

            var httpRequest = QueryViewHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await ViewQueryResponseFactory.CreateAsync<TValue>(res).ForAwait();
            }
        }

        public virtual async Task<ViewQueryResponse<TValue, TIncludedDoc>> QueryAsync<TValue, TIncludedDoc>(QueryViewRequest request, CancellationToken cancellationToken = default)
        {
            EnsureThatNoListFunctionIsUsed(request);

            var httpRequest = QueryViewHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await ViewQueryResponseFactory.CreateAsync<TValue, TIncludedDoc>(res).ForAwait();
        }

        private static void EnsureThatNoListFunctionIsUsed(QueryViewRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));
            if (!string.IsNullOrWhiteSpace(request.ListName))
                throw new NotSupportedException(
                    "This method does not support list functions to be used. Use QueryRawAsync instead.");
        }
    }
}