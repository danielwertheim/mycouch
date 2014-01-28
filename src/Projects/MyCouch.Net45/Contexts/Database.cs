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
        protected ContentResponseFactory ContentResponseFactory { get; set; }
        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Database(IConnection connection, ISerializer serializer) : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            ContentResponseFactory = new ContentResponseFactory(serializer);
            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
        }

        public virtual Task<ContentResponse> HeadAsync()
        {
            return HeadAsync(new HeadDatabaseRequest());
        }

        public virtual async Task<ContentResponse> HeadAsync(HeadDatabaseRequest request)
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

        public virtual Task<ContentResponse> GetAsync()
        {
            return GetAsync(new GetDatabaseRequest());
        }

        public virtual async Task<ContentResponse> GetAsync(GetDatabaseRequest request)
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

        public virtual Task<ContentResponse> PutAsync()
        {
            return PutAsync(new PutDatabaseRequest());
        }

        public virtual async Task<ContentResponse> PutAsync(PutDatabaseRequest request)
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

        public virtual Task<ContentResponse> DeleteAsync()
        {
            return DeleteAsync(new DeleteDatabaseRequest());
        }

        public virtual async Task<ContentResponse> DeleteAsync(DeleteDatabaseRequest request)
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

        public virtual Task<ContentResponse> CompactAsync()
        {
            return CompactAsync(new CompactDatabaseRequest());
        }

        public virtual async Task<ContentResponse> CompactAsync(CompactDatabaseRequest request)
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

        public virtual Task<ContentResponse> ViewCleanup()
        {
            return ViewCleanup(new ViewCleanupRequest());
        }

        public virtual async Task<ContentResponse> ViewCleanup(ViewCleanupRequest request)
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

        protected virtual ContentResponse ProcessResponse(HttpResponseMessage response)
        {
            return ContentResponseFactory.Create(response);
        }
    }
}