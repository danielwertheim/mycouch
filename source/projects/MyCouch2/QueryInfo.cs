using System;

namespace MyCouch
{
#if net45
    [Serializable]
#endif
    public class QueryInfo
    {
        public long TotalRows { get; private set; }
        public string UpdateSeq { get; private set; }
        public long RowCount { get; private set; }
        public long OffSet { get; private set; }

        public QueryInfo(long totalRows, long rowCount, long offSet, string updateSeq)
        {
            TotalRows = totalRows;
            RowCount = rowCount;
            OffSet = offSet;
            UpdateSeq = updateSeq;
        }
    }
}