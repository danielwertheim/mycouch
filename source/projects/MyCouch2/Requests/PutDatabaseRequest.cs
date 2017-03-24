namespace MyCouch.Requests
{
    public class PutDatabaseRequest : DatabaseRequest
    {
        public PutDatabaseRequest(string dbName) : base(dbName) { }
    }
}