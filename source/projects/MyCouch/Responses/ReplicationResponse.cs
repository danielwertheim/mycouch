using Newtonsoft.Json;

namespace MyCouch.Responses
{
    public class ReplicationResponse : Response,
        IDocumentHeader
    {
        [JsonProperty(JsonScheme.Id)]
        public string Id { get; set; }

        [JsonProperty(JsonScheme.Rev)]
        public string Rev { get; set; }
    }
}