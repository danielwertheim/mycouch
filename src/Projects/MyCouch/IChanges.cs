using System;
using System.Threading;
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
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>Only supports Normal and Long-polling feed. For Continuous feed, see <see cref="GetAsync(GetChangesRequest, Action{string}, CancellationToken)"/>.</remarks>
        Task<ChangesResponse> GetAsync(GetChangesRequest request);
        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// Included doc will be deserialized as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc">The type used to deserialize any included doc as.
        /// Supports string for JSON, which is the same as using the non generic overload.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>Only supports Normal and Long-polling feed. For Continuous feed, see <see cref="GetAsync(GetChangesRequest, Action{string}, CancellationToken)"/>.</remarks>
        Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request);

        /// <summary>
        /// Lets you consume changes continuously from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onRead">Callback invoked when data is retrieved from the stream.</param>
        /// <param name="cancellationToken">Used to end the reading of the stream.</param>
        /// <returns></returns>
        Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken);
    }
}