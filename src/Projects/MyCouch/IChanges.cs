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
        /// Lets you consume changes from the _changes stream, by using
        /// default values of CouchDb.
        /// </summary>
        /// <returns></returns>
        Task<ChangesResponse> GetAsync();
        /// <summary>
        /// Lets you consume changes from the _changes stream, by using
        /// default values of CouchDb.
        /// Included doc will be deserialized as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc">The type used to deserialize any included doc as.
        /// Supports string for JSON, which is the same as using the non generic overload.</typeparam>
        /// <returns></returns>
        Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>();
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
        /// Use the <paramref name="feed"/> param to define how
        /// you want to consume it.
        /// For more options, use the overload <see cref="GetAsync(GetChangesRequest)"/>.
        /// Included doc will be deserialized as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc">The type used to deserialize any included doc as.
        /// Supports string for JSON, which is the same as using the non generic overload.</typeparam>
        /// <param name="feed"></param>
        /// <returns></returns>
        Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(ChangesFeed feed);
        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ChangesResponse> GetAsync(GetChangesRequest request);
        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// Included doc will be deserialized as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc">The type used to deserialize any included doc as.
        /// Supports string for JSON, which is the same as using the non generic overload.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request);
    }
}