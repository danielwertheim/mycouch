using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Querying;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase, IViews
    {
        protected JsonViewQueryResponseFactory JsonViewQueryResponseFactory { get; set; }
        protected ViewQueryResponseFactory ViewQueryResponseFactory { get; set; }

        public Views(IConnection connection, SerializationConfiguration serializationConfiguration)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            JsonViewQueryResponseFactory = new JsonViewQueryResponseFactory(serializationConfiguration);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(serializationConfiguration);
        }

        public virtual async Task<JsonViewQueryResponse> QueryAsync(ViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(ViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);
            
            return ProcessHttpResponse<T>(await res.ForAwait());
        }

        public virtual Task<JsonViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<ViewQueryConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return QueryAsync(query);
        }

        public virtual Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<ViewQueryConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return QueryAsync<T>(query);
        }

        protected virtual ViewQuery CreateQuery(string designDocument, string viewname)
        {
            return new ViewQuery(designDocument, viewname);
        }
        
        protected virtual HttpRequestMessage CreateRequest(ViewQuery query)
        {
            return query.Options.HasKeys
                ? new HttpRequest(HttpMethod.Post, GenerateRequestUrl(query)).SetContent(query.Options.GetKeysAsJson())
                : new HttpRequest(HttpMethod.Get, GenerateRequestUrl(query));
        }

        protected virtual string GenerateRequestUrl(ViewQuery query)
        {
            if (query is SystemViewQuery)
            {
                return string.Format("{0}/{1}?{2}",
                    Connection.Address,
                    query.View.Name,
                    GenerateQueryStringParams(query.Options));
            }

            return string.Format("{0}/_design/{1}/_view/{2}?{3}",
                Connection.Address,
                query.View.DesignDocument,
                query.View.Name,
                GenerateQueryStringParams(query.Options));
        }

        protected virtual string GenerateQueryStringParams(ViewQueryOptions options)
        {
            return string.Join("&", options.ToJsonKeyValues()
                .Where(kv => kv.Key != ViewQueryOptions.KeyValues.Keys)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual JsonViewQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return JsonViewQueryResponseFactory.Create(response);
        }

        protected virtual ViewQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response)
        {
            return ViewQueryResponseFactory.Create<T>(response);
        }
    }
}