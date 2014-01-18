using System;
using MyCouch.Extensions;
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
    public class ViewQueryResponse<TValue> : ViewQueryResponse<TValue, string> { }

#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse<TValue, TIncludedDoc> : Response
    {
        [JsonProperty(JsonScheme.TotalRows)]
        public long TotalRows { get; set; }
        public long RowCount { get { return IsEmpty ? 0 : Rows.Length; } }
        [JsonProperty(JsonScheme.UpdateSeq)]
        public string UpdateSeq { get; set; }
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
            public TValue Value { get; set; }

            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public TIncludedDoc IncludedDoc { get; set; }

            public string KeyAsString()
            {
                return Key == null ? null : Key.ToString();
            }

            public Guid? KeyAsGuid()
            {
                return Key == null
                    ? null
                    : (Guid?)Guid.Parse(Key.ToString());
            }

            public DateTime? KeyAsDateTime()
            {
                return Key == null
                    ? null
                    : (DateTime?)Key.ToString().AsDateTimeFromIso8601();
            }

            public int? KeyAsInt()
            {
                return Key == null
                    ? null
                    : (int?)int.Parse(Key.ToString(), MyCouchRuntime.NumberFormat);
            }

            public long? KeyAsLong()
            {
                return Key == null
                    ? null
                    : (long?)long.Parse(Key.ToString(), MyCouchRuntime.NumberFormat);
            }

            public double? KeyAsDouble()
            {
                return Key == null
                    ? null
                    : (double?)double.Parse(Key.ToString(), MyCouchRuntime.NumberFormat);
            }

            public decimal? KeyAsDecimal()
            {
                return Key == null
                    ? null
                    : (decimal?)decimal.Parse(Key.ToString(), MyCouchRuntime.NumberFormat);
            }
        }
    }
}