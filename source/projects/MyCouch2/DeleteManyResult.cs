using System;

namespace MyCouch
{
#if net45
    [Serializable]
#endif
    public class DeleteManyResult
    {
        public Row[] Rows { get; set; }

        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }

#if net45
    [Serializable]
#endif
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