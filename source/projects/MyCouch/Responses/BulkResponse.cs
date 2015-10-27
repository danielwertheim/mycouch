﻿using System;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class BulkResponse : Response
    {
        public Row[] Rows { get; set; }
        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }

#if !PCL && !vNext
        [Serializable]
#endif
        public class Row
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Error { get; set; }
            public string Reason { get; set; }

            public bool Succeeded
            {
                get { return !string.IsNullOrWhiteSpace(Rev); }
            }
        }
    }
}