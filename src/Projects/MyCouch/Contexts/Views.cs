using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Configurators;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Views : ApiContextBase, IViews
    {
        protected IHttpRequestFactory<QueryViewRequest> HttpRequestFactory { get; set; }
        protected ViewQueryResponseFactory ViewQueryResponseFactory { get; set; }

        public Views(IConnection connection, SerializationConfiguration serializationConfiguration)
            : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            HttpRequestFactory = new QueryViewHttpRequestFactory(Connection);
            ViewQueryResponseFactory = new ViewQueryResponseFactory(serializationConfiguration);
        }

        public virtual async Task<ViewQueryResponse> QueryAsync(QueryViewRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        public virtual async Task<ViewQueryResponse<T>> QueryAsync<T>(QueryViewRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessHttpResponse<T>(res);
                }
            }
        }

        public virtual Task<ViewQueryResponse> QueryAsync(string designDocument, string viewname, Action<QueryViewRequestConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var request = CreateQueryViewRequest(designDocument, viewname);

            request.Configure(configurator);

            return QueryAsync(request);
        }

        public virtual Task<ViewQueryResponse<T>> QueryAsync<T>(string designDocument, string viewname, Action<QueryViewRequestConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewname, "viewname").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var request = CreateQueryViewRequest(designDocument, viewname);

            request.Configure(configurator);

            return QueryAsync<T>(request);
        }

        protected virtual QueryViewRequest CreateQueryViewRequest(string designDocument, string viewname)
        {
            return new QueryViewRequest(designDocument, viewname);
        }
        
        protected virtual HttpRequest CreateHttpRequest(QueryViewRequest request)
        {
            return HttpRequestFactory.Create(request);
        }

        protected virtual ViewQueryResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return ViewQueryResponseFactory.Create(response);
        }

        protected virtual ViewQueryResponse<T> ProcessHttpResponse<T>(HttpResponseMessage response)
        {
            return ViewQueryResponseFactory.Create<T>(response);
        }
    }
}