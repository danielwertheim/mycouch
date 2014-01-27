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
        protected PutDatabaseHttpRequestFactory PutDatabaseHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteDatabaseHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactDatabaseHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Database(IConnection connection, ISerializer serializer) : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseResponseFactory = new DatabaseResponseFactory(serializer);
            PutDatabaseHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteDatabaseHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactDatabaseHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
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

        protected virtual HttpRequest CreateHttpRequest(PutDatabaseRequest request)
        {
            return PutDatabaseHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteDatabaseRequest request)
        {
            return DeleteDatabaseHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(CompactDatabaseRequest request)
        {
            return CompactDatabaseHttpRequestFactory.Create(request);
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