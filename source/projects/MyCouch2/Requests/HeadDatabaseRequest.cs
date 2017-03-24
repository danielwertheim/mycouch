using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class HeadDatabaseRequest : DatabaseRequest
    {
        public HeadDatabaseRequest(string dbName) : base(dbName) { }
    }
}