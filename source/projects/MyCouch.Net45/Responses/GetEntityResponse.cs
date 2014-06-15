using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class GetEntityResponse<T> : EntityResponse<T>,
        IDocumentHeader
        where T : class
    {
        [JsonProperty(JsonScheme._Id)]
        public override string Id { get; set; }

        [JsonProperty(JsonScheme._Rev)]
        public override string Rev { get; set; }

        [JsonProperty(JsonScheme.Conflicts)]
        public virtual string[] Conflicts { get; set; }
    }
}