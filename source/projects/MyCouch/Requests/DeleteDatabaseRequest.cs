using System;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class DeleteDatabaseRequest : DatabaseRequest
    {
        public DeleteDatabaseRequest(string dbName) : base(dbName) { }
    }
}