using System.Threading.Tasks;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        Task<SearchIndexResponse> SearchAsync(SearchIndexRequest search);
        Task<SearchIndexResponse<TIncludedDoc>> SearchAsync<TIncludedDoc>(SearchIndexRequest search);
    }
}