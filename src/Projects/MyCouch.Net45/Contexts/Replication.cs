namespace MyCouch.Contexts
{
    public class Replication : ApiContextBase, IReplication
    {
        public Replication(IConnection connection) : base(connection) { }
    }
}