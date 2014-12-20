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
    public class Lists : ApiContextBase<IDbClientConnection>, ILists
    {
        protected QueryListHttpRequestFactory QueryListHttpRequestFactory { get; set; }
        protected ListQueryResponseFactory ListQueryResponseFactory { get; set; }

        public Lists(IDbClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            QueryListHttpRequestFactory = new QueryListHttpRequestFactory(serializer);
            ListQueryResponseFactory = new ListQueryResponseFactory(serializer);
        }

        //TODO: Add cancellationtoken as in views. Should be added in searches as well.
        public virtual async Task<ListQueryResponse> QueryAsync(QueryListRequest request)
        {
            var httpRequest = CreateHttpRequest(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return ProcessListHttpResponse(res);
            }
        }

        protected virtual HttpRequest CreateHttpRequest(QueryListRequest request)
        {
            return QueryListHttpRequestFactory.Create(request);
        }

        protected virtual ListQueryResponse ProcessListHttpResponse(HttpResponseMessage response)
        {
            return ListQueryResponseFactory.Create(response);
        }
    }
}