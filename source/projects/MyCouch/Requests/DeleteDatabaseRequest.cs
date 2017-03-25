namespace MyCouch.Requests
{
    public class DeleteDatabaseRequest : DatabaseRequest
    {
        public DeleteDatabaseRequest(string dbName) : base(dbName) { }
    }
}