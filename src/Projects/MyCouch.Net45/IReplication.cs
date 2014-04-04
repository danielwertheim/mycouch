using MyCouch.Requests;

namespace MyCouch
{
    public interface IReplication
    {
        object ReplicateAsync(ReplicateDatabaseRequest request);
    }
}