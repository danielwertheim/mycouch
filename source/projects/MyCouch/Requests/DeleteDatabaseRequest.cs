using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class DeleteDatabaseRequest : DatabaseRequest
    {
        public DeleteDatabaseRequest(string dbName) : base(dbName) { }
    }
}