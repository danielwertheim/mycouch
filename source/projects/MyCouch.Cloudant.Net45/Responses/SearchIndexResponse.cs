using System;
using System.Collections.Generic;
using System.Linq;
using MyCouch.Responses;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Cloudant.Responses
{
#if !PCL
    [Serializable]
#endif
    public class SearchIndexResponse : SearchIndexResponse<string> { }

#if !PCL
    [Serializable]
#endif
    public class SearchIndexResponse<TIncludedDoc> : Response
    {
        public Row[] Rows { get; set; }
        public Group[] Groups { get; set; }
        [JsonProperty(JsonScheme.TotalRows)]
        public long TotalRows { get; set; }
        public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        public long GroupCount { get { return IsGroupsEmpty ? 0 : Groups.Length; } }
        public bool IsEmpty
        {
            get { return Rows == null || Rows.Length == 0; }
        }
        public bool IsGroupsEmpty
        {
            get { return Groups == null || Groups.Length == 0; }
        }
        public string Bookmark { get; set; }
        
        [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
        public string Counts { get; set; }
        
        [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
        public string Ranges { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{0}{1}{0}IsEmpty: {2}{0}TotalRows: {3}{0}RowCount: {4}{0}Bookmark: {5}{0}GroupCount: {6}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                IsEmpty,
                TotalRows,
                RowCount,
                Bookmark,
                GroupCount);
        }
#if !PCL
        [Serializable]
#endif
        public class Row
        {
            public string Id { get; set; }
            public object[] Order { get; set; }
            public Dictionary<string, object> Fields { get; set; }

            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public TIncludedDoc IncludedDoc { get; set; }

            public virtual double[] GetOrderAsDoubles()
            {
                return Order == null || Order.Length == 0
                    ? new double[0] : Order.Cast<double>().ToArray();
            }
        }

#if !PCL
        [Serializable]
#endif
        public class Group
        {
            public string By { get; set; }
            [JsonProperty(JsonScheme.TotalRows)]
            public long TotalRows { get; set; }
            public Row[] Rows { get; set; }
            public bool IsEmpty
            {
                get { return Rows == null || Rows.Length == 0; }
            }
            public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        }
    }
}