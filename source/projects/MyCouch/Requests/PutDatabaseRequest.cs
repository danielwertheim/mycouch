using System;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class PutDatabaseRequest : DatabaseRequest
    {
        public PutDatabaseRequest(string dbName) : base(dbName) { }
    }
}