using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCouch.Requests
{
    public class PurgeRequest : Request
    {
        [JsonIgnore]
        public Dictionary<string, string[]> SeqsById { get; set; } = new Dictionary<string, string[]>();

        public PurgeRequest(string id, string[] revs) => SeqsById.Add(id, revs);

        public PurgeRequest(string id, string rev) => SeqsById.Add(id, new[] { rev });

        public PurgeRequest() {}
        

        // because JsonExtensionData do not support other than JToken as value

        [JsonExtensionData]
        private IDictionary<string, JToken> _extensionData;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => SeqsById = _extensionData.ToDictionary(o => o.Key, o => o.Value.ToObject<string[]>());

        [OnSerializing]
        private void OnSerializing(StreamingContext context) => _extensionData = SeqsById.ToDictionary(o => o.Key, o => (JToken)JArray.FromObject(o.Value));
    }
}