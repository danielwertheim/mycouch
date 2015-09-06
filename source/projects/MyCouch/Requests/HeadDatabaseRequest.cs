using System;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class HeadDatabaseRequest : DatabaseRequest
    {
        public HeadDatabaseRequest(string dbName) : base(dbName) { }
    }
}