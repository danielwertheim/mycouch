using System.Threading.Tasks;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISearches
    {
        /// <summary>
        /// Lets you run a <see cref="IndexQuery"/>.
        /// The resulting <see cref="JsonIndexQueryResponse"/> will consist of
        /// Rows being JSON-strings.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<JsonIndexQueryResponse> RunQueryAsync(IndexQuery query);

        /// <summary>
        /// Lets you run a <see cref="IndexQuery"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IndexQueryResponse<T>> RunQueryAsync<T>(IndexQuery query) where T : class;
    }
}