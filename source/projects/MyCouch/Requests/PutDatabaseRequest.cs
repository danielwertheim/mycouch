using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class PutDatabaseRequest : DatabaseRequest
    {
        public PutDatabaseRequest(string dbName) : base(dbName) { }
    }
}