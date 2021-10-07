using MyCouch.Requests;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
    public class PurgeResponse : Response
    {
        [JsonProperty(JsonScheme.PurgeSeq)]
        public string PurgeSeq { get; set; }
        public PurgeRequest Purged { get; set; }
    }
}