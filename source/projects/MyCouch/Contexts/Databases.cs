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
    public class Databases : ApiContextBase<IServerConnection>, IDatabases
    {
        protected DatabaseHeaderResponseFactory DatabaseHeaderResponseFactory { get; set; }
        protected GetDatabaseResponseFactory GetDatabaseResponseFactory { get; set; }

        protected GetDatabaseServerHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseServerHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseServerHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseServerHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseServerHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupServerHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Databases(IServerConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            DatabaseHeaderResponseFactory = new DatabaseHeaderResponseFactory(serializer);
            GetDatabaseResponseFactory = new GetDatabaseResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseServerHttpRequestFactory();
            HeadHttpRequestFactory = new HeadDatabaseServerHttpRequestFactory();
            PutHttpRequestFactory = new PutDatabaseServerHttpRequestFactory();
            DeleteHttpRequestFactory = new DeleteDatabaseServerHttpRequestFactory();
            CompactHttpRequestFactory = new CompactDatabaseServerHttpRequestFactory();
            ViewCleanupHttpRequestFactory = new ViewCleanupServerHttpRequestFactory();
        }

        public virtual Task<GetDatabaseResponse> GetAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return GetAsync(new GetDatabaseRequest(dbName), cancellationToken);
        }

        public virtual async Task<GetDatabaseResponse> GetAsync(GetDatabaseRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = GetHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await GetDatabaseResponseFactory.CreateAsync(httpResponse, cancellationToken).ForAwait();
        }

        public virtual Task<DatabaseHeaderResponse> HeadAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return HeadAsync(new HeadDatabaseRequest(dbName), cancellationToken);
        }

        public virtual async Task<DatabaseHeaderResponse> HeadAsync(HeadDatabaseRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = HeadHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse, cancellationToken).ForAwait();
        }

        public virtual Task<DatabaseHeaderResponse> PutAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return PutAsync(new PutDatabaseRequest(dbName), cancellationToken);
        }

        public virtual async Task<DatabaseHeaderResponse> PutAsync(PutDatabaseRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = PutHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse, cancellationToken).ForAwait();
        }

        public virtual Task<DatabaseHeaderResponse> DeleteAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return DeleteAsync(new DeleteDatabaseRequest(dbName), cancellationToken);
        }

        public virtual async Task<DatabaseHeaderResponse> DeleteAsync(DeleteDatabaseRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = DeleteHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse, cancellationToken).ForAwait();
        }

        public virtual Task<DatabaseHeaderResponse> CompactAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return CompactAsync(new CompactDatabaseRequest(dbName), cancellationToken);
        }

        public virtual async Task<DatabaseHeaderResponse> CompactAsync(CompactDatabaseRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = CompactHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse, cancellationToken).ForAwait();
        }

        public virtual Task<DatabaseHeaderResponse> ViewCleanupAsync(string dbName, CancellationToken cancellationToken = default)
        {
            return ViewCleanupAsync(new ViewCleanupRequest(dbName), cancellationToken);
        }

        public virtual async Task<DatabaseHeaderResponse> ViewCleanupAsync(ViewCleanupRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = ViewCleanupHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, cancellationToken).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse, cancellationToken).ForAwait();
        }
    }
}