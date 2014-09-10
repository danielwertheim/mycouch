
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;
using System.Threading.Tasks;
namespace MyCouch.Cloudant
{
    /// <summary>
    /// Used to create, delete, list and query indexes at Cloudant
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
    }
}
