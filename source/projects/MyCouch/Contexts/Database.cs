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
    public class Database : ApiContextBase<IDbConnection>, IDatabase
    {
        protected DatabaseHeaderResponseFactory DatabaseHeaderResponseFactory { get; set; }
        protected GetDatabaseResponseFactory GetDatabaseResponseFactory { get; set; }

        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Database(IDbConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseHeaderResponseFactory = new DatabaseHeaderResponseFactory(serializer);
            GetDatabaseResponseFactory = new GetDatabaseResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory();
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory();
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory();
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory();
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory();
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory();
        }

        public virtual async Task<DatabaseHeaderResponse> HeadAsync()
        {
            var request = new HeadDatabaseRequest(Connection.DbName);
            var httpRequest = HeadHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse).ForAwait();
        }

        public virtual async Task<GetDatabaseResponse> GetAsync()
        {
            var request = new GetDatabaseRequest(Connection.DbName);
            var httpRequest = GetHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await GetDatabaseResponseFactory.CreateAsync(httpResponse).ForAwait();
        }

        public virtual async Task<DatabaseHeaderResponse> PutAsync()
        {
            var request = new PutDatabaseRequest(Connection.DbName);
            var httpRequest = PutHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse).ForAwait();
        }

        public virtual async Task<DatabaseHeaderResponse> DeleteAsync()
        {
            var request = new DeleteDatabaseRequest(Connection.DbName);
            var httpRequest = DeleteHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse).ForAwait();
        }

        public virtual async Task<DatabaseHeaderResponse> CompactAsync()
        {
            var request = new CompactDatabaseRequest(Connection.DbName);
            var httpRequest = CompactHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse).ForAwait();
        }

        public virtual async Task<DatabaseHeaderResponse> ViewCleanupAsync()
        {
            var request = new ViewCleanupRequest(Connection.DbName);
            var httpRequest = ViewCleanupHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return await DatabaseHeaderResponseFactory.CreateAsync(request, httpResponse).ForAwait();
        }
    }
}