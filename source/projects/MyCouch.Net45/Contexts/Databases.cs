using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Databases : ApiContextBase<IServerClientConnection>, IDatabases
    {
        protected DatabaseHeaderResponseFactory DatabaseHeaderResponseFactory { get; set; }
        protected GetDatabaseResponseFactory GetDatabaseResponseFactory { get; set; }
        protected ReplicationResponseFactory ReplicationResponseFactory { get; set; }

        protected GetDatabaseServerHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseServerHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseServerHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseServerHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseServerHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupServerHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }
        protected ReplicateDatabaseServerHttpRequestFactory ReplicateDatabaseHttpRequestFactory { get; set; }

        public Databases(IServerClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseHeaderResponseFactory = new DatabaseHeaderResponseFactory(serializer);
            GetDatabaseResponseFactory = new GetDatabaseResponseFactory(serializer);
            ReplicationResponseFactory = new ReplicationResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseServerHttpRequestFactory();
            HeadHttpRequestFactory = new HeadDatabaseServerHttpRequestFactory();
            PutHttpRequestFactory = new PutDatabaseServerHttpRequestFactory();
            DeleteHttpRequestFactory = new DeleteDatabaseServerHttpRequestFactory();
            CompactHttpRequestFactory = new CompactDatabaseServerHttpRequestFactory();
            ViewCleanupHttpRequestFactory = new ViewCleanupServerHttpRequestFactory();
            ReplicateDatabaseHttpRequestFactory = new ReplicateDatabaseServerHttpRequestFactory(serializer);
        }

        public virtual Task<GetDatabaseResponse> GetAsync(string dbName)
        {
            return GetAsync(new GetDatabaseRequest(dbName));
        }

        public virtual async Task<GetDatabaseResponse> GetAsync(GetDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessGetDatabaseResponse(res);
            }
        }

        public virtual Task<DatabaseHeaderResponse> HeadAsync(string dbName)
        {
            return HeadAsync(new HeadDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> HeadAsync(HeadDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual Task<DatabaseHeaderResponse> PutAsync(string dbName)
        {
            return PutAsync(new PutDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> PutAsync(PutDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual Task<DatabaseHeaderResponse> DeleteAsync(string dbName)
        {
            return DeleteAsync(new DeleteDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> DeleteAsync(DeleteDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual Task<DatabaseHeaderResponse> CompactAsync(string dbName)
        {
            return CompactAsync(new CompactDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> CompactAsync(CompactDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual Task<DatabaseHeaderResponse> ViewCleanupAsync(string dbName)
        {
            return ViewCleanupAsync(new ViewCleanupRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> ViewCleanupAsync(ViewCleanupRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual Task<ReplicationResponse> ReplicateAsync(string source, string target)
        {
            return ReplicateAsync(new ReplicateDatabaseRequest(source, target));
        }

        public virtual async Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessReplicationResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(GetDatabaseRequest request)
        {
            return GetHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(HeadDatabaseRequest request)
        {
            return HeadHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(PutDatabaseRequest request)
        {
            return PutHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteDatabaseRequest request)
        {
            return DeleteHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(CompactDatabaseRequest request)
        {
            return CompactHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(ViewCleanupRequest request)
        {
            return ViewCleanupHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(ReplicateDatabaseRequest request)
        {
            return ReplicateDatabaseHttpRequestFactory.Create(request);
        }

        protected virtual DatabaseHeaderResponse ProcessDatabaseHeaderResponse(HttpResponseMessage response)
        {
            return DatabaseHeaderResponseFactory.Create(response);
        }

        protected virtual GetDatabaseResponse ProcessGetDatabaseResponse(HttpResponseMessage response)
        {
            return GetDatabaseResponseFactory.Create(response);
        }

        protected virtual ReplicationResponse ProcessReplicationResponse(HttpResponseMessage response)
        {
            return ReplicationResponseFactory.Create(response);
        }
    }
}