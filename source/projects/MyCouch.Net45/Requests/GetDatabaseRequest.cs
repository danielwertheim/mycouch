using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class GetDatabaseRequest : DatabaseRequest
    {
        public GetDatabaseRequest(string dbName) : base(dbName) { }
    }
}