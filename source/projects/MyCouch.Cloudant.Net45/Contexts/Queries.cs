using EnsureThat;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Cloudant.Contexts
{
    public class Queries : ApiContextBase<IDbClientConnection>, IQueries
    {
        protected PostIndexHttpRequestFactory PostIndexHttpRequestFactory { get; set; }
        protected GetAllIndexesHttpRequestFactory GetAllIndexesHttpRequestFactory { get; set; }
        protected DeleteIndexHttpRequestFactory DeleteIndexHttpRequestFactory { get; set; }
        protected IndexResponseFactory IndexResponseFactory { get; set; }
        protected IndexListResponseFactory IndexListResponseFactory { get; set; }
        public Queries(IDbClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            PostIndexHttpRequestFactory = new PostIndexHttpRequestFactory(serializer);
            GetAllIndexesHttpRequestFactory = new GetAllIndexesHttpRequestFactory();
            DeleteIndexHttpRequestFactory = new DeleteIndexHttpRequestFactory();

            IndexResponseFactory = new IndexResponseFactory(serializer);
            IndexListResponseFactory = new IndexListResponseFactory(serializer);
        }

        public virtual async Task<IndexResponse> PostAsync(PostIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexResponse(res);
            }
        }

        public virtual async Task<IndexListResponse> GetAllAsync()
        {
            var httpRequest = CreateHttpRequest();

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexListResponse(res);
            }
        }

        public virtual async Task<IndexResponse> DeleteAsync(DeleteIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessIndexResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteIndexRequest request)
        {
            return DeleteIndexHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest()
        {
            return GetAllIndexesHttpRequestFactory.Create();
        }

        protected virtual HttpRequest CreateHttpRequest(PostIndexRequest request)
        {
            return PostIndexHttpRequestFactory.Create(request);
        }

        protected virtual IndexResponse ProcessIndexResponse(HttpResponseMessage response)
        {
            return IndexResponseFactory.Create(response);
        }

        protected virtual IndexListResponse ProcessIndexListResponse(HttpResponseMessage response)
        {
            return IndexListResponseFactory.Create(response);
        }
    }
}
