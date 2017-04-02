namespace MyCouch.Requests
{
    public class HeadDatabaseRequest : DatabaseRequest
    {
        public HeadDatabaseRequest(string dbName) : base(dbName) { }
    }
}