using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class GetDatabaseRequest : DatabaseRequest
    {
        public GetDatabaseRequest(string dbName) : base(dbName) { }
    }
}