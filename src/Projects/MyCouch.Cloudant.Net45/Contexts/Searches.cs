using System.Threading.Tasks;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Requests.Factories;
using MyCouch.Cloudant.Responses;
using MyCouch.Contexts;

namespace MyCouch.Cloudant.Contexts
{
    public class Searches : ApiContextBase, ISearches
    {
        protected SearchIndexHttpRequestFactory SearchIndexHttpRequestFactory { get; set; }

        public Searches(IConnection connection) : base(connection)
        {
            SearchIndexHttpRequestFactory = new SearchIndexHttpRequestFactory(Connection);
        }

        public virtual Task<SearchIndexResponse> SearchAsync(SearchIndexRequest search)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest search)
        {
            throw new System.NotImplementedException();
        }
    }
}