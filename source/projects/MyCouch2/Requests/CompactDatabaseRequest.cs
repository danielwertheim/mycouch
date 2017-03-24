using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class CompactDatabaseRequest : DatabaseRequest
    {
        public CompactDatabaseRequest(string dbName) : base(dbName) { }
    }
}