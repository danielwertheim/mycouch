using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCouch.Responses
{
    public class PurgeResponse : Response
    {
        [JsonProperty(JsonScheme.PurgeSeq)]
        public string PurgeSeq { get; set; }
        public PurgedData Purged { get; set; }
    }

    public class PurgedData 
    {
        [JsonIgnore]
        public Dictionary<string, string[]> SeqsById { get; set; } = new Dictionary<string, string[]>();


        // because JsonExtensionData do not support other than JToken as value

        [JsonExtensionData]
        private IDictionary<string, JToken> _extensionData = null;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => SeqsById = _extensionData?.ToDictionary(o => o.Key, o => o.Value.ToObject<string[]>());
    }
}