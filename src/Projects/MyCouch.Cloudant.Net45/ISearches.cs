using System.Threading.Tasks;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        Task<SearchIndexResponse> SearchAsync(SearchIndexRequest search);
        Task<SearchIndexResponse<TValue>> SearchAsync<TValue>(SearchIndexRequest search);
        Task<SearchIndexResponse<TValue, TIncludedDoc>> SearchAsync<TValue, TIncludedDoc>(SearchIndexRequest search);
    }
}