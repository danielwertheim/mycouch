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
    public class Database : ApiContextBase, IDatabase
    {
        protected DatabaseResponseFactory DatabaseResponseFactory { get; set; }
        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Database(IConnection connection, ISerializer serializer) : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseResponseFactory = new DatabaseResponseFactory(serializer);
            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
        }

        public virtual Task<DatabaseResponse> HeadAsync()
        {
            return HeadAsync(new HeadDatabaseRequest());
        }

        public virtual async Task<DatabaseResponse> HeadAsync(HeadDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual Task<DatabaseResponse> GetAsync()
        {
            return GetAsync(new GetDatabaseRequest());
        }

        public virtual async Task<DatabaseResponse> GetAsync(GetDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual Task<DatabaseResponse> PutAsync()
        {
            return PutAsync(new PutDatabaseRequest());
        }

        public virtual async Task<DatabaseResponse> PutAsync(PutDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual Task<DatabaseResponse> DeleteAsync()
        {
            return DeleteAsync(new DeleteDatabaseRequest());
        }

        public virtual async Task<DatabaseResponse> DeleteAsync(DeleteDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual Task<DatabaseResponse> CompactAsync()
        {
            return CompactAsync(new CompactDatabaseRequest());
        }

        public virtual async Task<DatabaseResponse> CompactAsync(CompactDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual Task<DatabaseResponse> ViewCleanup()
        {
            return ViewCleanup(new ViewCleanupRequest());
        }

        public virtual async Task<DatabaseResponse> ViewCleanup(ViewCleanupRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
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

        protected virtual DatabaseResponse ProcessResponse(HttpResponseMessage response)
        {
            return DatabaseResponseFactory.Create(response);
        }
    }
}