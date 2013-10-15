using System;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class QueryResponseRow
    {
        public string Id { get; set; }
        [JsonConverter(typeof(KeyJsonConverter))]
        public object Key { get; set; }
    }
}