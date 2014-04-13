using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class ViewCleanupRequest : Request
    {
        public string DbName { get; private set; }

        public ViewCleanupRequest(string dbName)
        {
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName;
        }
    }
}