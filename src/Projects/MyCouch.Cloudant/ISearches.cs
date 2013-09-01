using System.Threading.Tasks;
using MyCouch.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        /// <summary>
        /// Lets you run a <see cref="IndexQuery"/>.
        /// The resulting <see cref="JsonViewQueryResponse"/> will consist of
        /// Rows being JSON-strings.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<JsonViewQueryResponse> RunQueryAsync(IndexQuery query);

        /// <summary>
        /// Lets you run a <see cref="IndexQuery"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ViewQueryResponse<T>> RunQueryAsync<T>(IndexQuery query) where T : class;
    }
}