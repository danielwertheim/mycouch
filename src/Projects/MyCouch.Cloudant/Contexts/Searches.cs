using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Cloudant.Querying;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase, ISearches
    {
        protected readonly JsonSearchQueryResponseFactory JsonSearchQueryResponseFactory;
        protected readonly SearchQueryResponseFactory SearchQueryResponseFactory;

        public Searches(IConnection connection, SerializationConfiguration serializationConfiguration) 
            : base(connection)
        {
            JsonSearchQueryResponseFactory = new JsonSearchQueryResponseFactory(serializationConfiguration);
            SearchQueryResponseFactory = new SearchQueryResponseFactory(serializationConfiguration);
        }

        public virtual async Task<JsonSearchQueryResponse> QueryAsync(SearchQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual Task<SearchQueryResponse<T>> QueryAsync<T>(SearchQuery query) where T : class
        {
            Ensure.That(query, "query").IsNotNull();

            throw new NotImplementedException();
        }

        protected virtual SearchQuery CreateQuery(string designDocument, string viewname)
        {
            return new SearchQuery(designDocument, viewname);
        }

        protected virtual HttpRequestMessage CreateRequest(SearchQuery query)
        {
            return new HttpRequest(HttpMethod.Get, GenerateRequestUrl(query));
        }

        protected virtual string GenerateRequestUrl(SearchQuery query)
        {
            return string.Format("{0}/_design/{1}/_search/{2}?{3}",
                Connection.Address,
                query.Index.DesignDocument,
                query.Index.Name,
                GenerateQueryStringParams(query.Options));
        }

        protected virtual string GenerateQueryStringParams(SearchQueryOptions options)
        {
            return string.Join("&", options.ToKeyValues().Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual JsonSearchQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return JsonSearchQueryResponseFactory.Create(response);
        }

        protected virtual SearchQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response) where T : class
        {
            return SearchQueryResponseFactory.Create<T>(response);
        }
    }
}