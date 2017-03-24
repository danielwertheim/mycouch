using System;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class ViewCleanupRequest : DatabaseRequest
    {
        public ViewCleanupRequest(string dbName) : base(dbName) { }
    }
}