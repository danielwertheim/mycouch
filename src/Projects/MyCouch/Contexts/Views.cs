using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Querying;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase, IViews
    {
        protected ViewQueryRequestBuilder RequestBuilder { get; set; }
        protected JsonViewQueryResponseFactory JsonViewQueryResponseFactory { get; set; }
        protected ViewQueryResponseFactory ViewQueryResponseFactory { get; set; }

        public Views(IConnection connection, SerializationConfiguration serializationConfiguration)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            RequestBuilder = new ViewQueryRequestBuilder(Connection);
            JsonViewQueryResponseFactory = new JsonViewQueryResponseFactory(serializationConfiguration);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(serializationConfiguration);
        }

        public virtual async Task<JsonViewQueryResponse> QueryAsync(ViewQuery query)
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

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(ViewQuery query)
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

        protected virtual ViewQuery CreateQuery(string designDocument, string viewname)
        {
            return new ViewQuery(designDocument, viewname);
        }
        
        protected virtual HttpRequestMessage CreateRequest(ViewQuery query)
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