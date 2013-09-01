using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase, IViews
    {
        protected readonly JsonViewQueryResponseFactory JsonViewQueryResponseFactory;
        protected readonly ViewQueryResponseFactory ViewQueryResponseFactory;

        public Views(IConnection connection, SerializationConfiguration serializationConfiguration)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            var materializer = new DefaultResponseMaterializer(serializationConfiguration);
            JsonViewQueryResponseFactory = new JsonViewQueryResponseFactory(materializer);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(materializer);
        }

        public virtual async Task<JsonViewQueryResponse> RunQueryAsync(IViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual async Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class
        {
            Ensure.That(query, "query").IsNotNull();

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