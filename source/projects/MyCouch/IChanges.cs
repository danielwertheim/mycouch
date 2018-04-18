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
        /// Resolves the <see cref="TaskFactory"/> used when running observable queries.
        /// By default <see cref="Task.Factory"/>.
        /// </summary>
        Func<TaskFactory> ObservableWorkTaskFactoryResolver { set; }

        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>Only supports Normal and Long-polling feed. For Continuous feed, see <see cref="ObserveContinuous"/>.</remarks>
        Task<ChangesResponse> GetAsync(GetChangesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets you consume changes from the _changes stream.
        /// Included doc will be deserialized as <typeparamref name="TIncludedDoc"/>.
        /// </summary>
        /// <typeparam name="TIncludedDoc">The type used to deserialize any included doc as.
        /// Supports string for JSON, which is the same as using the non generic overload.</typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>Only supports Normal and Long-polling feed. For Continuous feed, see <see cref="ObserveContinuous"/>.</remarks>
        Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets you consume changes continuously from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onRead">Callback invoked when data is retrieved from the stream.</param>
        /// <param name="cancellationToken">Used to end the reading of the stream.</param>
        /// <returns></returns>
        Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets you consume changes continuously from the _changes stream and handle the result in a Task based approach.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onRead">Callback invoked when data is retrieved from the stream.</param>
        /// <param name="cancellationToken">Used to end the reading of the stream.</param>
        /// <returns></returns>
        Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Func<string, Task> onRead, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets you consume changes continuously from the _changes stream.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Used to end the reading of the stream.</param>
        /// <returns></returns>
        IObservable<string> ObserveContinuous(GetChangesRequest request, CancellationToken cancellationToken = default);
    }
}