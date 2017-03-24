using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class PutDatabaseRequest : DatabaseRequest
    {
        public PutDatabaseRequest(string dbName) : base(dbName) { }
    }
}