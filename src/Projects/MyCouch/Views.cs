using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Responses.ResponseFactories;

namespace MyCouch
{
    public class Views : IViews
    {
        protected readonly IConnection Connection;
        protected readonly JsonViewQueryResponseFactory JsonViewQueryResponseFactory;
        protected readonly ViewQueryResponseFactory ViewQueryResponseFactory;

        public Views(IConnection connection, JsonViewQueryResponseFactory jsonViewQueryResponseFactory, ViewQueryResponseFactory viewQueryResponseFactory)
        {
            Ensure.That(connection, "connection").IsNotNull();
            Ensure.That(jsonViewQueryResponseFactory, "jsonViewQueryResponseFactory").IsNotNull();
            Ensure.That(viewQueryResponseFactory, "viewQueryResponseFactory").IsNotNull();

            Connection = connection;
            JsonViewQueryResponseFactory = jsonViewQueryResponseFactory;
            ViewQueryResponseFactory = viewQueryResponseFactory;
        }

        public virtual async Task<JsonViewQueryResponse> RunQueryAsync(IViewQuery query)
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual async Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = SendAsync(req);
            
            return ProcessHttpResponse<T>(await res.ForAwait());
        }

        public virtual Task<JsonViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return RunQueryAsync(query);
        }

        public virtual Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return RunQueryAsync<T>(query);
        }

        protected virtual void EnsureValidQuery(IViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();
        }

        protected virtual IViewQuery CreateQuery(string designDocument, string viewname)
        {
            return new ViewQuery(designDocument, viewname);
        }
        
        protected virtual HttpRequestMessage CreateRequest(IViewQuery query)
        {
            return new HttpRequest(HttpMethod.Get, GenerateRequestUrl(query));
        }

        protected virtual string GenerateRequestUrl(IViewQuery query)
        {
            if (query is ISystemViewQuery)
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

        protected virtual string GenerateQueryStringParams(IViewQueryOptions options)
        {
            return string.Join("&", options.ToKeyValues().Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Connection.SendAsync(request);
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