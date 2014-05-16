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
    public class Database : ApiContextBase<IDbClientConnection>, IDatabase
    {
        protected DatabaseHeaderResponseFactory DatabaseHeaderResponseFactory { get; set; }
        protected GetDatabaseResponseFactory GetDatabaseResponseFactory { get; set; }

        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Database(IDbClientConnection connection, ISerializer serializer)
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
            var httpRequest = CreateHttpRequest(new HeadDatabaseRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual async Task<GetDatabaseResponse> GetAsync()
        {
            var httpRequest = CreateHttpRequest(new GetDatabaseRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessGetDatabaseResponse(res);
            }
        }

        public virtual async Task<DatabaseHeaderResponse> PutAsync()
        {
            var httpRequest = CreateHttpRequest(new PutDatabaseRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual async Task<DatabaseHeaderResponse> DeleteAsync()
        {
            var httpRequest = CreateHttpRequest(new DeleteDatabaseRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual async Task<DatabaseHeaderResponse> CompactAsync()
        {
            var httpRequest = CreateHttpRequest(new CompactDatabaseRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
            }
        }

        public virtual async Task<DatabaseHeaderResponse> ViewCleanupAsync()
        {
            var httpRequest = CreateHttpRequest(new ViewCleanupRequest(Connection.DbName));

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessDatabaseHeaderResponse(res);
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

        protected virtual DatabaseHeaderResponse ProcessDatabaseHeaderResponse(HttpResponseMessage response)
        {
            return DatabaseHeaderResponseFactory.Create(response);
        }

        protected virtual GetDatabaseResponse ProcessGetDatabaseResponse(HttpResponseMessage response)
        {
            return GetDatabaseResponseFactory.Create(response);
        }
    }
}