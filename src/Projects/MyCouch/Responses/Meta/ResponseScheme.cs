namespace MyCouch.Responses.Meta
{
    public class ResponseScheme
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public string Error { get; private set; }
        public string Reason { get; private set; }
        public QueriesScheme Queries { get; private set; }

        public ResponseScheme()
        {
            Id = "id";
            Rev = "rev";
            Error = "error";
            Reason = "reason";
            Queries = new QueriesScheme();
        }
    }
}
