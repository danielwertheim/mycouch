
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
        Task<IndexResponse> PostAsync(IndexRequest request);
    }
}
