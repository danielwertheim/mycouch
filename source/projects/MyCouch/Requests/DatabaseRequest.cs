using EnsureThat;

namespace MyCouch.Requests
{
    public abstract class DatabaseRequest : Request
    {
        public string DbName { get; private set; }

        protected DatabaseRequest(string dbName)
        {
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName;
        }
    }
}