using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class QueryResponse<TRow> : Response where TRow : QueryResponseRow
    {
        public long TotalRows { get; set; }
        public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        public long UpdateSeq { get; set; }
        public long OffSet { get; set; }
        public TRow[] Rows { get; set; }
        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}TotalRows: {3}{0}RowCount: {4}{0}Offset: {5}{0}UpdateSeq: {6}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty,
                TotalRows,
                RowCount,
                OffSet,
                UpdateSeq);
        }
    }

#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class QueryResponseRow
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }
}
