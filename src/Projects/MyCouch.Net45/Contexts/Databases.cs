using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Databases : ApiContextBase<IServerClientConnection>, IDatabases
    {
        protected TextResponseFactory TextResponseFactory { get; set; }
        protected ReplicationResponseFactory ReplicationResponseFactory { get; set; }

        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }
        protected ReplicateDatabaseHttpRequestFactory ReplicateDatabaseHttpRequestFactory { get; set; }

        public Databases(IServerClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            TextResponseFactory = new TextResponseFactory(serializer);
            ReplicationResponseFactory = new ReplicationResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
            ReplicateDatabaseHttpRequestFactory = new ReplicateDatabaseHttpRequestFactory(Connection);
        }

        public virtual Task<TextResponse> HeadAsync(string dbName)
        {
            return HeadAsync(new HeadDatabaseRequest(dbName));
        }

        public virtual async Task<TextResponse> HeadAsync(HeadDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<TextResponse> GetAsync(string dbName)
        {
            return GetAsync(new GetDatabaseRequest(dbName));
        }

        public virtual async Task<TextResponse> GetAsync(GetDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<TextResponse> PutAsync(string dbName)
        {
            return PutAsync(new PutDatabaseRequest(dbName));
        }

        public virtual async Task<TextResponse> PutAsync(PutDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<TextResponse> DeleteAsync(string dbName)
        {
            return DeleteAsync(new DeleteDatabaseRequest(dbName));
        }

        public virtual async Task<TextResponse> DeleteAsync(DeleteDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<TextResponse> CompactAsync(string dbName)
        {
            return CompactAsync(new CompactDatabaseRequest(dbName));
        }

        public virtual async Task<TextResponse> CompactAsync(CompactDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<TextResponse> ViewCleanupAsync(string dbName)
        {
            return ViewCleanupAsync(new ViewCleanupRequest(dbName));
        }

        public virtual async Task<TextResponse> ViewCleanupAsync(ViewCleanupRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual Task<ReplicationResponse> ReplicateAsync(string source, string target)
        {
            return ReplicateAsync(new ReplicateDatabaseRequest(source, target));
        }

        public virtual async Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessReplicationResponse(res);
                }
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

        protected virtual TextResponse ProcessTextResponse(HttpResponseMessage response)
        {
            return TextResponseFactory.Create(response);
        }

        protected virtual ReplicationResponse ProcessReplicationResponse(HttpResponseMessage response)
        {
            return ReplicationResponseFactory.Create(response);
        }
    }
}