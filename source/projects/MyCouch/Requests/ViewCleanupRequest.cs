using System;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class ViewCleanupRequest : DatabaseRequest
    {
        public ViewCleanupRequest(string dbName) : base(dbName) { }
    }
}