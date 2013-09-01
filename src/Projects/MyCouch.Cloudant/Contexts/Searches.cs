using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Cloudant.Querying;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase, ISearches
    {
        protected readonly JsonViewQueryResponseFactory JsonViewQueryResponseFactory;
        protected readonly ViewQueryResponseFactory ViewQueryResponseFactory;

        public Searches(IConnection connection, SerializationConfiguration serializationConfiguration) 
            : base(connection)
        {
            var materializer = new DefaultResponseMaterializer(serializationConfiguration);
            JsonViewQueryResponseFactory = new JsonViewQueryResponseFactory(materializer);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(materializer);
        }

        public virtual async Task<JsonViewQueryResponse> RunQueryAsync(IndexQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual Task<ViewQueryResponse<T>> RunQueryAsync<T>(IndexQuery query) where T : class
        {
            Ensure.That(query, "query").IsNotNull();

            throw new NotImplementedException();
        }

        protected virtual IndexQuery CreateQuery(string designDocument, string viewname)
        {
            return new IndexQuery(designDocument, viewname);
        }

        protected virtual HttpRequestMessage CreateRequest(IndexQuery query)
        {
            return new HttpRequest(HttpMethod.Get, GenerateRequestUrl(query));
        }

        protected virtual string GenerateRequestUrl(IndexQuery query)
        {
            return string.Format("{0}/_design/{1}/_search/{2}?{3}",
                Connection.Address,
                query.Index.DesignDocument,
                query.Index.Name,
                GenerateQueryStringParams(query.Options));
        }

        protected virtual string GenerateQueryStringParams(IndexQueryOptions options)
        {
            return string.Join("&", options.ToKeyValues().Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual JsonViewQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return JsonViewQueryResponseFactory.Create(response);
        }

        protected virtual ViewQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response) where T : class
        {
            return ViewQueryResponseFactory.Create<T>(response);
        }
    }
}