using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
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