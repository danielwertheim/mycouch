using System.Threading.Tasks;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        /// <summary>
        /// Lets you run a <see cref="SearchQuery"/>.
        /// The resulting <see cref="JsonSearchQueryResponse"/> will consist of
        /// Rows being JSON-strings.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<JsonSearchQueryResponse> QueryAsync(SearchQuery query);

        /// <summary>
        /// Lets you run a <see cref="SearchQuery"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<SearchQueryResponse<T>> QueryAsync<T>(SearchQuery query) where T : class;
    }
}