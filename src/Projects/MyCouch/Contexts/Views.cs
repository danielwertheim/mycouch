using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Querying;
using MyCouch.Requests;
using MyCouch.Requests.Builders;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase, IViews
    {
        protected QueryViewRequestBuilder RequestBuilder { get; set; }
        protected JsonViewQueryResponseFactory JsonViewQueryResponseFactory { get; set; }
        protected ViewQueryResponseFactory ViewQueryResponseFactory { get; set; }

        public Views(IConnection connection, SerializationConfiguration serializationConfiguration)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            RequestBuilder = new QueryViewRequestBuilder(Connection);
            JsonViewQueryResponseFactory = new JsonViewQueryResponseFactory(serializationConfiguration);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(serializationConfiguration);
        }

        public virtual async Task<JsonViewQueryResponse> QueryAsync(QueryViewRequest query)
        {
            Ensure.That(query, "query").IsNotNull();

            using (var req = CreateRequest(query))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(QueryViewRequest query)
        {
            Ensure.That(query, "query").IsNotNull();

            using (var req = CreateRequest(query))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessHttpResponse<T>(res);
                }
            }
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

        protected virtual QueryViewRequest CreateQuery(string designDocument, string viewname)
        {
            return new QueryViewRequest(designDocument, viewname);
        }
        
        protected virtual HttpRequestMessage CreateRequest(QueryViewRequest query)
        {
            return RequestBuilder.Create(query);
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