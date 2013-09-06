namespace MyCouch.Responses.Meta
{
    public class QueriesScheme
    {
        public string TotalRows { get; private set; }
        public string UpdateSeq { get; private set; }
        public string Offset { get; private set; }
        public string Rows { get; private set; }
        public string RowId { get; private set; }
        public string RowKey { get; private set; }
        public string RowValue { get; private set; }
        public string RowDoc { get; private set; }

        public QueriesScheme()
        {
            TotalRows = "total_rows";
            UpdateSeq = "update_seq";
            Offset = "offset";
            Rows = "rows";
            RowId = "id";
            RowKey = "key";
            RowValue = "value";
            RowDoc = "doc";
        }
    }
}