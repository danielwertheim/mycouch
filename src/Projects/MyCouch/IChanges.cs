using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    /// <summary>
    /// Used to consume the changes feed.
    /// </summary>
    public interface IChanges
    {
        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// Use the <paramref name="feed"/> param to define how
        /// you want to consume it.
        /// For more options, use the overload <see cref="GetAsync(GetChangesRequest)"/>.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        Task<ChangesResponse> GetAsync(ChangesFeed feed);
        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ChangesResponse> GetAsync(GetChangesRequest request);
    }
}