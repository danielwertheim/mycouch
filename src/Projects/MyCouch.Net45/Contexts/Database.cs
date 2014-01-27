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

        public virtual async Task<DatabaseResponse> PutAsync()
        {
            using (var req = CreateHttpRequest(new PutDatabaseRequest()))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> DeleteAsync()
        {
            using (var req = CreateHttpRequest(new DeleteDatabaseRequest()))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> CompactAsync()
        {
            using (var req = CreateHttpRequest(new CompactDatabaseRequest()))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<DatabaseResponse> ViewCleanup()
        {
            using (var req = CreateHttpRequest(new ViewCleanupRequest()))
            {
                using (var res = await SendAsync(req).ForAwait())
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