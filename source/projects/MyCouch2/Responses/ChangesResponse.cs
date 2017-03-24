using System;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public class ChangesResponse : ChangesResponse<string> { }

#if net45
    [Serializable]
#endif
    public class ChangesResponse<TIncludedDoc> : Response
    {
        [JsonProperty(JsonScheme.LastSeq)]
        public string LastSeq { get; set; }
        public Row[] Results { get; set; }

#if net45
        [Serializable]
#endif
        public class Change
        {
            public string Rev { get; set; }
        }

#if net45
        [Serializable]
#endif
        public class Row
        {
            public virtual string Id { get; set; }
            public virtual string Seq { get; set; }
            public virtual Change[] Changes { get; set; }
            public virtual bool Deleted { get; set; }
            [JsonProperty(JsonScheme.IncludedDoc)]
            [JsonConverter(typeof(MultiTypeDeserializationJsonConverter))]
            public virtual TIncludedDoc IncludedDoc { get; set; }
        }
    }
}