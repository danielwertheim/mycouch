namespace MyCouch.Requests
{
    public class GetDatabaseRequest : DatabaseRequest
    {
        public GetDatabaseRequest(string dbName) : base(dbName) { }
    }
}