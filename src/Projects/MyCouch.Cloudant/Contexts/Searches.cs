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
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase, ISearches
    {
        protected readonly JsonIndexQueryResponseFactory JsonIndexQueryResponseFactory;
        protected readonly IndexQueryResponseFactory IndexQueryResponseFactory;

        public Searches(IConnection connection, SerializationConfiguration serializationConfiguration) 
            : base(connection)
        {
            var materializer = new DefaultResponseMaterializer(serializationConfiguration);
            JsonIndexQueryResponseFactory = new JsonIndexQueryResponseFactory(materializer);
            IndexQueryResponseFactory = new IndexQueryResponseFactory(materializer);
        }

        public virtual async Task<JsonIndexQueryResponse> RunQueryAsync(IndexQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual Task<IndexQueryResponse<T>> RunQueryAsync<T>(IndexQuery query) where T : class
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

        protected virtual JsonIndexQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return JsonIndexQueryResponseFactory.Create(response);
        }

        protected virtual IndexQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response) where T : class
        {
            return IndexQueryResponseFactory.Create<T>(response);
        }
    }
}