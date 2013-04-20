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

        public virtual IViewQuery CreateQuery(string designDocument, string viewname)
        {
            return new ViewQuery(Client, designDocument, viewname);
        }

        public virtual ViewQueryResponse RunQuery(IViewQuery query)
        {
            return RunQueryAsync(query).Result;
        }

        public virtual ViewQueryResponse<T> RunQuery<T>(IViewQuery query) where T : class
        {
            return RunQueryAsync<T>(query).Result;
        }

        public virtual async Task<ViewQueryResponse> RunQueryAsync(IViewQuery query)
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);

            return await ProcessHttpResponseAsync(SendAsync(req));
        }

        public virtual async Task<ViewQueryResponse<T>> RunQueryAsync<T>(IViewQuery query) where T : class
        {
            Ensure.That(query, "query").IsNotNull();

            var req = CreateRequest(query);

            return await ProcessHttpResponseAsync<T>(SendAsync(req));
        }

        public virtual ViewQueryResponse Query(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return RunQuery(query);
        }

        public virtual ViewQueryResponse<T> Query<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            return QueryAsync<T>(designDocument, viewname, configurator).Result;
        }

        public virtual async Task<ViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator)
        {
            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return await RunQueryAsync(query);
        }

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<IViewQueryConfigurator> configurator) where T : class
        {
            var query = CreateQuery(designDocument, viewname);

            query.Configure(configurator);

            return await RunQueryAsync<T>(query);
        }

        protected virtual HttpRequestMessage CreateRequest(IViewQuery query)
        {
            return new HttpRequest(HttpMethod.Get, GenerateRequestUrl(query));
        }

        protected virtual string GenerateRequestUrl(IViewQuery query)
        {
            return string.Format("{0}/_design/{1}/_view/{2}?{3}",
                Client.Connection.Address,
                query.DesignDocument,
                query.ViewName,
                GenerateQueryStringParams(query));
        }

        protected virtual string GenerateQueryStringParams(IViewQuery query)
        {
            return string.Join("&", query.Options.Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        protected virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await Client.Connection.SendAsync(request);
        }

        protected virtual async Task<ViewQueryResponse> ProcessHttpResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateViewQueryResponse(await responseTask);
        }

        protected virtual async Task<ViewQueryResponse<T>> ProcessHttpResponseAsync<T>(Task<HttpResponseMessage> responseTask) where T : class 
        {
            return Client.ResponseFactory.CreateViewQueryResponse<T>(await responseTask);
        }
    }
}