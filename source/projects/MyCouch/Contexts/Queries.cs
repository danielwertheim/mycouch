using System.Threading;
using EnsureThat;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Extensions;
using MyCouch.Serialization;
using System.Threading.Tasks;

namespace MyCouch.Contexts
{
    public class Queries : ApiContextBase<IDbConnection>, IQueries
    {
        protected PostIndexHttpRequestFactory PostIndexHttpRequestFactory { get; set; }
        protected GetAllIndexesHttpRequestFactory GetAllIndexesHttpRequestFactory { get; set; }
        protected DeleteIndexHttpRequestFactory DeleteIndexHttpRequestFactory { get; set; }
        protected FindHttpRequestFactory FindHttpRequestFactory { get; set; }
        protected IndexResponseFactory IndexResponseFactory { get; set; }
        protected IndexListResponseFactory IndexListResponseFactory { get; set; }
        protected FindResponseFactory FindResponseFactory { get; set; }

        public Queries(IDbConnection connection, ISerializer documentSerializer, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(documentSerializer, nameof(documentSerializer));
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            PostIndexHttpRequestFactory = new PostIndexHttpRequestFactory(serializer);
            GetAllIndexesHttpRequestFactory = new GetAllIndexesHttpRequestFactory();
            DeleteIndexHttpRequestFactory = new DeleteIndexHttpRequestFactory();
            FindHttpRequestFactory = new FindHttpRequestFactory(serializer);

            IndexResponseFactory = new IndexResponseFactory(serializer);
            IndexListResponseFactory = new IndexListResponseFactory(serializer);
            FindResponseFactory = new FindResponseFactory(documentSerializer);
        }

        public virtual async Task<IndexResponse> PostAsync(PostIndexRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = PostIndexHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await IndexResponseFactory.CreateAsync(res, cancellationToken).ForAwait();
            }
        }

        public virtual async Task<IndexListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var httpRequest = GetAllIndexesHttpRequestFactory.Create();

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await IndexListResponseFactory.CreateAsync(res, cancellationToken).ForAwait();
            }
        }

        public virtual async Task<IndexResponse> DeleteAsync(DeleteIndexRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = DeleteIndexHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await IndexResponseFactory.CreateAsync(res, cancellationToken).ForAwait();
            }
        }

        public virtual async Task<FindResponse> FindAsync(FindRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = FindHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await FindResponseFactory.CreateAsync(res, cancellationToken).ForAwait();
            }
        }

        public virtual async Task<FindResponse<TIncludedDoc>> FindAsync<TIncludedDoc>(FindRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = FindHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await FindResponseFactory.CreateAsync<TIncludedDoc>(res, cancellationToken).ForAwait();
            }
        }
    }
}
