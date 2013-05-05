using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Querying;

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
            return RunQueryAsync(query).Result;
        }

        public virtual async Task<JsonViewQueryResponse> RunQueryAsync(IViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);

            return await ProcessHttpResponseAsync(res);
        }

        public virtual ViewQueryResponse<T> RunQuery<T>(IViewQuery query) where T : class
        {
            return RunQueryAsync<T>(query).Result;
        }

        public virtual async Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);
            var res = SendAsync(req);
            
            return await ProcessHttpResponseAsync<T>(res);
        }

        public virtual JsonViewQueryResponse Query(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            return QueryAsync(designDocument, viewname, configurator).Result;
        }

        public virtual async Task<JsonViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return await RunQueryAsync(query);
        }

        public virtual ViewQueryResponse<T> Query<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            return QueryAsync<T>(designDocument, viewname, configurator).Result;
        }

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return await RunQueryAsync<T>(query);
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

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual async Task<JsonViewQueryResponse> ProcessHttpResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateJsonViewQueryResponse(await responseTask);
        }

        protected virtual async Task<ViewQueryResponse<T>> ProcessHttpResponseAsync<T>(Task<HttpResponseMessage> responseTask) where T : class 
        {
            return Client.ResponseFactory.CreateViewQueryResponse<T>(await responseTask);
        }
    }
}