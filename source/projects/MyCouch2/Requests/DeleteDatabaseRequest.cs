using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class DeleteDatabaseRequest : DatabaseRequest
    {
        public DeleteDatabaseRequest(string dbName) : base(dbName) { }
    }
}