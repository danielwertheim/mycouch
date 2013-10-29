using System;
using System.Collections.Generic;
using MyCouch.Responses;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Cloudant.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchIndexResponse : SearchIndexResponse<string> { }

#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchIndexResponse<TIncludedDoc> : Response
    {
        [JsonProperty(JsonScheme.TotalRows)]
        public long TotalRows { get; set; }
        public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        public Row[] Rows { get; set; }
        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }
        public string Bookmark { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}TotalRows: {3}{0}RowCount: {4}{0}Bookmark: {5}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty,
                TotalRows,
                RowCount,
                Bookmark);
        }
#if !NETFX_CORE
        [Serializable]
#endif
        public class Row : IResponseRow
        {
            public string Id { get; set; }
            public double[] Order { get; set; }
            public Dictionary<string, object> Fields { get; set; }

            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public TIncludedDoc IncludedDoc { get; set; }
        }
    }
}