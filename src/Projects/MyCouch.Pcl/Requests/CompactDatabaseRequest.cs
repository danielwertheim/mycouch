using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class CompactDatabaseRequest : Request
    {
        public string DbName { get; private set; }

        public CompactDatabaseRequest(string dbName)
        {
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName;
        }
    }
}