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
        protected IndexHttpRequestFactory IndexHttpRequestFactory { get; set; }
        protected IndexResponseFactory IndexResponseFactory { get; set; }
        public Queries(IDbClientConnection connection, ISerializer documentSerializer, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(documentSerializer, "documentSerializer").IsNotNull();
            Ensure.That(serializer, "serializer").IsNotNull();

            IndexHttpRequestFactory = new IndexHttpRequestFactory(serializer);
            IndexResponseFactory = new IndexResponseFactory(documentSerializer);
        }

        public virtual async Task<IndexResponse> PostAsync(PostIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessHttpResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(PostIndexRequest request)
        {
            return IndexHttpRequestFactory.Create(request);
        }

        protected virtual IndexResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return IndexResponseFactory.Create(response);
        }
    }
}
