using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class HeadDatabaseRequest : DatabaseRequest
    {
        public HeadDatabaseRequest(string dbName) : base(dbName) { }
    }
}