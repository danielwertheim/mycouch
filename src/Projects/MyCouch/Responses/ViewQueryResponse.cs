using System;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse : ViewQueryResponse<string> { }

#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse<T> : Response
    {
        [JsonProperty(JsonScheme.TotalRows)]
        public long TotalRows { get; set; }
        public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        [JsonProperty(JsonScheme.UpdateSeq)]
        public long UpdateSeq { get; set; }
        public long OffSet { get; set; }
        public Row[] Rows { get; set; }
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
#if !NETFX_CORE
        [Serializable]
#endif
        public class Row : IResponseRow
        {
            public string Id { get; set; }
            
            [JsonConverter(typeof(KeyJsonConverter))]
            public object Key { get; set; }
            
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public T Value { get; set; }
            
            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public T IncludedDoc { get; set; }
        }
    }
}