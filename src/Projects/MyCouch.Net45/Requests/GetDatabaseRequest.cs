using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class GetDatabaseRequest : Request
    {
        public string DbName { get; private set; }

        public GetDatabaseRequest(string dbName)
        {
            Ensure.That(dbName, "dbName").IsNotNullOrWhiteSpace();

            DbName = dbName;
        }
    }
}