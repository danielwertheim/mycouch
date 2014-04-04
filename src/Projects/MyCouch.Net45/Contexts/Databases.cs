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

            var dbRequestUrlGenerator = new AppendingDbRequestUrlGenerator(Connection.Address);

            TextResponseFactory = new TextResponseFactory(serializer);
            GetHttpRequestFactory = new GetDatabaseHttpRequestFactory(Connection, dbRequestUrlGenerator);
            HeadHttpRequestFactory = new HeadDatabaseHttpRequestFactory(Connection, dbRequestUrlGenerator);
            PutHttpRequestFactory = new PutDatabaseHttpRequestFactory(Connection, dbRequestUrlGenerator);
            DeleteHttpRequestFactory = new DeleteDatabaseHttpRequestFactory(Connection, dbRequestUrlGenerator);
            CompactHttpRequestFactory = new CompactDatabaseHttpRequestFactory(Connection, dbRequestUrlGenerator);
            ViewCleanupHttpRequestFactory = new ViewCleanupHttpRequestFactory(Connection, dbRequestUrlGenerator);
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
                    return ProcessResponse(res);
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
                    return ProcessResponse(res);
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
                    return ProcessResponse(res);
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
                    return ProcessResponse(res);
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
                    return ProcessResponse(res);
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