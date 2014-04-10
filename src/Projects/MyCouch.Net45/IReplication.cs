using System.Threading.Tasks;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch
{
    public interface IReplication
    {
        Task<ReplicationResponse> ReplicateAsync(string source, string target);
        Task<ReplicationResponse> ReplicateAsync(ReplicateDatabaseRequest request);
    }
}