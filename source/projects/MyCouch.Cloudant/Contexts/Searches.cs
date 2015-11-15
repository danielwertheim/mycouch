using System.Threading.Tasks;
using MyCouch.Cloudant.HttpRequestFactories;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using MyCouch.Cloudant.Responses.Factories;
using MyCouch.Contexts;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Serialization;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase<IDbConnection>, ISearches
    {
        protected SearchIndexHttpRequestFactory SearchIndexHttpRequestFactory { get; set; }
        protected SearchIndexResponseFactory SearchIndexResponseFactory { get; set; }

        public Searches(IDbConnection connection, ISerializer documentSerializer, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(documentSerializer, "documentSerializer").IsNotNull();
            Ensure.That(serializer, "serializer").IsNotNull();

            SearchIndexHttpRequestFactory = new SearchIndexHttpRequestFactory(serializer);
            SearchIndexResponseFactory = new SearchIndexResponseFactory(documentSerializer);
        }

        public virtual async Task<SearchIndexResponse> SearchAsync(SearchIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = SearchIndexHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await SearchIndexResponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = SearchIndexHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await SearchIndexResponseFactory.CreateAsync<TIncludedDoc>(res).ForAwait();
            }
        }
    }
}