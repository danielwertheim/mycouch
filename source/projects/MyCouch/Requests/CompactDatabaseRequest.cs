namespace MyCouch.Requests
{
    public class CompactDatabaseRequest : DatabaseRequest
    {
        public CompactDatabaseRequest(string dbName) : base(dbName) { }
    }
}