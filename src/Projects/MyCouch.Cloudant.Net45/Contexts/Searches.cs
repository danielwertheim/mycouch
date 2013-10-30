using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Requests.Configurators;
using MyCouch.Cloudant.Requests.Factories;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase, ISearches
    {
        protected SearchIndexHttpRequestFactory SearchIndexHttpRequestFactory { get; set; }
        protected SearchIndexResponseFactory SearchIndexResponseFactory { get; set; }

        public Searches(IConnection connection, ISerializer serializer, IEntitySerializer entitySerializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();
            Ensure.That(entitySerializer, "entitySerializer").IsNotNull();

            SearchIndexHttpRequestFactory = new SearchIndexHttpRequestFactory(Connection);
            SearchIndexResponseFactory = new SearchIndexResponseFactory(serializer, entitySerializer);
        }

        public virtual async Task<SearchIndexResponse> SearchAsync(SearchIndexRequest request)
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

        public virtual async Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessHttpResponse<TIncludedDoc>(res);
                }
            }
        }

        public virtual Task<SearchIndexResponse> SearchAsync(string designDocument, string searchIndexName, Action<SearchIndexRequestConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(searchIndexName, "searchIndexName").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var request = CreateSearchIndexRequest(designDocument, searchIndexName);

            request.Configure(configurator);

            return SearchAsync(request);
        }

        public virtual Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(string designDocument, string searchIndexName, Action<SearchIndexRequestConfigurator> configurator)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(searchIndexName, "searchIndexName").IsNotNullOrWhiteSpace();
            Ensure.That(configurator, "configurator").IsNotNull();

            var request = CreateSearchIndexRequest(designDocument, searchIndexName);

            request.Configure(configurator);

            return SearchAsync<TIncludedDoc>(request);
        }

        protected virtual SearchIndexRequest CreateSearchIndexRequest(string designDocument, string searchIndexName)
        {
            return new SearchIndexRequest(designDocument, searchIndexName);
        }

        protected virtual HttpRequest CreateHttpRequest(SearchIndexRequest request)
        {
            return SearchIndexHttpRequestFactory.Create(request);
        }

        protected virtual SearchIndexResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            return SearchIndexResponseFactory.Create(response);
        }

        protected virtual SearchIndexResponse<TIncludedDoc> ProcessHttpResponse<TIncludedDoc>(HttpResponseMessage response)
        {
            return SearchIndexResponseFactory.Create<TIncludedDoc>(response);
        }
    }
}