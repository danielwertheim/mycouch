using System.Threading;
using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public interface IReplicator
    {
        /// <summary>
        /// Initiates a new Replication task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(string id, string source, string target, CancellationToken cancellationToken = default);

        /// <summary>
        /// Initiates a new Replication task.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request, CancellationToken cancellationToken = default);
    }
}