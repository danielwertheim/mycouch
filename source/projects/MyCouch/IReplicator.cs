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
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(string id, string source, string target);

        /// <summary>
        /// Initiates a new Replication task.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request);
    }
}