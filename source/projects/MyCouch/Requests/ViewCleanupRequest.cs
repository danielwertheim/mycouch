namespace MyCouch.Requests
{
    public class ViewCleanupRequest : DatabaseRequest
    {
        public ViewCleanupRequest(string dbName) : base(dbName) { }
    }
}