using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCouch 
{
    public class PurgeData
    {
        [JsonIgnore]
        public Dictionary<string, string[]> SeqsById { get; private set; }

        public PurgeData(string id, string rev) => SeqsById = new Dictionary<string, string[]>() { { id, new[] { rev } } };
        public PurgeData(Dictionary<string, string[]> seqsById) => SeqsById = seqsById;
        public PurgeData() {}


        // because JsonExtensionData do not support other than JToken as value

        [JsonExtensionData]
        private IDictionary<string, JToken> _extensionData = null;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => SeqsById = _extensionData?.ToDictionary(o => o.Key, o => o.Value.ToObject<string[]>());

        [OnSerializing]
        private void OnSerializing(StreamingContext context) => _extensionData = SeqsById?.ToDictionary(o => o.Key, o => (JToken)JArray.FromObject(o.Value));
    }
}