using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to create, delete, list and query indexes.
    /// </summary>
    public interface IQueries
    {
        /// <summary>
        /// Lets you create an index
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IndexResponse> PostAsync(PostIndexRequest request);

        /// <summary>
        /// Gets a list of all indexes in the database.
        /// </summary>
        /// <returns></returns>
        Task<IndexListResponse> GetAllAsync();

        /// <summary>
        /// Lets you delete an existing index
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IndexResponse> DeleteAsync(DeleteIndexRequest request);

        /// <summary>
        /// Lets you find documents by querying indexes using the
        /// Query API by using a reusable <see cref="FindRequest"/>.
        /// Any returned IncludedDoc will be treated as JSON.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FindResponse> FindAsync(FindRequest request);

        /// <summary>
        /// Lets you find documents by querying indexes using the
        /// Query API by using a reusable <see cref="FindRequest"/>.
        /// Any returned IncludedDoc will be treated as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FindResponse<TIncludedDoc>> FindAsync<TIncludedDoc>(FindRequest request);
    }
}
