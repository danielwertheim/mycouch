using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCouch.Requests
{
    public class PurgeDocumentRequest : Request
    {
        [JsonIgnore]
        public Dictionary<string, string[]> SeqsById { get; set; } = new Dictionary<string, string[]>();

        public PurgeDocumentRequest(string id, string[] revs) => SeqsById.Add(id, revs);

        public PurgeDocumentRequest(string id, string rev) => SeqsById.Add(id, new[] { rev });

        public PurgeDocumentRequest() {}
        

        // because JsonExtensionData do not support other than JToken as value

        [JsonExtensionData]
        private IDictionary<string, JToken> _extensionData;

        [OnSerializing]
        private void OnSerializing(StreamingContext context) => _extensionData = SeqsById.ToDictionary(o => o.Key, o => (JToken)JArray.FromObject(o.Value));
    }
}