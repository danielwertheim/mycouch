using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class ReplicationResponse : Response
    {
        [JsonProperty(JsonScheme.Id)]
        public string Id { get; set; }

        [JsonProperty(JsonScheme.Rev)]
        public string Rev { get; set; }
    }
}