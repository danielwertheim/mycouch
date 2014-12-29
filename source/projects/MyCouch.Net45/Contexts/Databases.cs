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
            Ensure.That(serializer, "serializer").IsNotNull();

            DatabaseHeaderResponseFactory = new DatabaseHeaderResponseFactory(serializer);
            GetDatabaseResponseFactory = new GetDatabaseResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseServerHttpRequestFactory();
            HeadHttpRequestFactory = new HeadDatabaseServerHttpRequestFactory();
            PutHttpRequestFactory = new PutDatabaseServerHttpRequestFactory();
            DeleteHttpRequestFactory = new DeleteDatabaseServerHttpRequestFactory();
            CompactHttpRequestFactory = new CompactDatabaseServerHttpRequestFactory();
            ViewCleanupHttpRequestFactory = new ViewCleanupServerHttpRequestFactory();
        }

        public virtual Task<GetDatabaseResponse> GetAsync(string dbName)
        {
            return GetAsync(new GetDatabaseRequest(dbName));
        }

        public virtual async Task<GetDatabaseResponse> GetAsync(GetDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessGetDatabaseResponse(httpResponse);
        }

        public virtual Task<DatabaseHeaderResponse> HeadAsync(string dbName)
        {
            return HeadAsync(new HeadDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> HeadAsync(HeadDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessDatabaseHeaderResponse(request, httpResponse);
        }

        public virtual Task<DatabaseHeaderResponse> PutAsync(string dbName)
        {
            return PutAsync(new PutDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> PutAsync(PutDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessDatabaseHeaderResponse(request, httpResponse);
        }

        public virtual Task<DatabaseHeaderResponse> DeleteAsync(string dbName)
        {
            return DeleteAsync(new DeleteDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> DeleteAsync(DeleteDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessDatabaseHeaderResponse(request, httpResponse);
        }

        public virtual Task<DatabaseHeaderResponse> CompactAsync(string dbName)
        {
            return CompactAsync(new CompactDatabaseRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> CompactAsync(CompactDatabaseRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessDatabaseHeaderResponse(request, httpResponse);
        }

        public virtual Task<DatabaseHeaderResponse> ViewCleanupAsync(string dbName)
        {
            return ViewCleanupAsync(new ViewCleanupRequest(dbName));
        }

        public virtual async Task<DatabaseHeaderResponse> ViewCleanupAsync(ViewCleanupRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var httpResponse = await SendAsync(httpRequest).ForAwait())
                return ProcessDatabaseHeaderResponse(request, httpResponse);
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

        protected virtual DatabaseHeaderResponse ProcessDatabaseHeaderResponse(DatabaseRequest request, HttpResponseMessage response)
        {
            return DatabaseHeaderResponseFactory.Create(request, response);
        }

        protected virtual GetDatabaseResponse ProcessGetDatabaseResponse(HttpResponseMessage response)
        {
            return GetDatabaseResponseFactory.Create(response);
        }
    }
}