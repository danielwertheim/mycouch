using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class CompactDatabaseRequest : DatabaseRequest
    {
        public CompactDatabaseRequest(string dbName) : base(dbName) { }
    }
}