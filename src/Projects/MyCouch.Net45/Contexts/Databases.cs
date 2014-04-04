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
        protected GetDatabaseHttpRequestFactory GetHttpRequestFactory { get; set; }
        protected HeadDatabaseHttpRequestFactory HeadHttpRequestFactory { get; set; }
        protected PutDatabaseHttpRequestFactory PutHttpRequestFactory { get; set; }
        protected DeleteDatabaseHttpRequestFactory DeleteHttpRequestFactory { get; set; }
        protected CompactDatabaseHttpRequestFactory CompactHttpRequestFactory { get; set; }
        protected ViewCleanupHttpRequestFactory ViewCleanupHttpRequestFactory { get; set; }

        public Databases(IServerClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            TextResponseFactory = new TextResponseFactory(serializer);
            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection);
        }

        public virtual async Task<TextResponse> HeadAsync(HeadDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> GetAsync(GetDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> PutAsync(PutDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> DeleteAsync(DeleteDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> CompactAsync(CompactDatabaseRequest request)
        {
            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessResponse(res);
                }
            }
        }

        public virtual async Task<TextResponse> ViewCleanup(ViewCleanupRequest request)
        {
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

        protected virtual TextResponse ProcessResponse(HttpResponseMessage response)
        {
            return TextResponseFactory.Create(response);
        }
    }
}