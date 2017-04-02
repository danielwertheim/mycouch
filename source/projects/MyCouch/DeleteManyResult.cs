namespace MyCouch
{
    public class DeleteManyResult
    {
        public Row[] Rows { get; set; }

        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }

        public class Row
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public bool Deleted { get; set; }
            public string Error { get; set; }
            public string Reason { get; set; }
        }
    }
}