using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Database : ApiContextBase<IDbClientConnection>, IDatabase
    {
        protected TextResponseFactory TextResponseFactory { get; set; }
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

            TextResponseFactory = new TextResponseFactory(serializer);
            GetDatabaseResponseFactory = new GetDatabaseResponseFactory(serializer);

            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
        }

        public virtual async Task<TextResponse> HeadAsync()
        {
            using (var httpRequest = CreateHttpRequest(new HeadDatabaseRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual async Task<GetDatabaseResponse> GetAsync()
        {
            using (var httpRequest = CreateHttpRequest(new GetDatabaseRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessGetDatabaseResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> PutAsync()
        {
            using (var httpRequest = CreateHttpRequest(new PutDatabaseRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> DeleteAsync()
        {
            using (var httpRequest = CreateHttpRequest(new DeleteDatabaseRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> CompactAsync()
        {
            using (var httpRequest = CreateHttpRequest(new CompactDatabaseRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> ViewCleanupAsync()
        {
            using (var httpRequest = CreateHttpRequest(new ViewCleanupRequest(Connection.DbName)))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessTextResponse(res);
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

        protected virtual TextResponse ProcessTextResponse(HttpResponseMessage response)
        {
            return TextResponseFactory.Create(response);
        }

        protected virtual GetDatabaseResponse ProcessGetDatabaseResponse(HttpResponseMessage response)
        {
            return GetDatabaseResponseFactory.Create(response);
        }
    }
}