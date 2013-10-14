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
    public class ViewQueryResponse<T> : QueryResponse<ViewQueryResponse<T>.Row>
    {
#if !NETFX_CORE
        [Serializable]
#endif
        public class Row : QueryResponseRow
        {
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public T Value { get; set; }
            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public T IncludedDoc { get; set; }
        }
    }
}