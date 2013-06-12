using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Core;
using MyCouch.Net;

namespace MyCouch
{
    public class Views : IViews
    {
        protected readonly IClient Client;

        public Views(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual JsonViewQueryResponse RunQuery(IViewQuery query)
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = Send(req);

            return ProcessHttpResponse(res);
        }

        public virtual async Task<JsonViewQueryResponse> RunQueryAsync(IViewQuery query)
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return ProcessHttpResponse(await res.ForAwait());
        }

        public virtual ViewQueryResponse<T> RunQuery<T>(IViewQuery query) where T : class
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = Send(req);

            return ProcessHttpResponse<T>(res);
        }

        public virtual async Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class
        {
            EnsureValidQuery(query);

            var req = CreateRequest(query);
            var res = SendAsync(req);
            
            return ProcessHttpResponse<T>(await res.ForAwait());
        }

        public virtual JsonViewQueryResponse Query(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return RunQuery(query);
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

        public virtual ViewQueryResponse<T> Query<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return RunQuery<T>(query);
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
                    Client.Connection.Address,
                    query.ViewName,
                    GenerateQueryStringParams(query.Options));
            }

            return string.Format("{0}/_design/{1}/_view/{2}?{3}",
                Client.Connection.Address,
                query.DesignDocument,
                query.ViewName,
                GenerateQueryStringParams(query.Options));
        }

        protected virtual string GenerateQueryStringParams(IViewQueryOptions options)
        {
            return string.Join("&", options.ToKeyValues().Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return Client.Connection.Send(request);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual JsonViewQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return Client.ResponseFactory.CreateJsonViewQueryResponse(response);
        }

        protected virtual ViewQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response) where T : class
        {
            return Client.ResponseFactory.CreateViewQueryResponse<T>(response);
        }
    }
}