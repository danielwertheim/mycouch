using MyCouch.Requests;

namespace MyCouch.Contexts
{
    public class Replication : ApiContextBase<IServerClientConnection>, IReplication
    {
        public Replication(IServerClientConnection connection) : base(connection) { }

        public object ReplicateAsync(ReplicateDatabaseRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}