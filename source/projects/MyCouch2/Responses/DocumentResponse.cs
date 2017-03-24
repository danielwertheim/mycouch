using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public class DocumentResponse : TextResponse,
        IDocumentHeader
    {
        [JsonProperty(JsonScheme._Id)]
        public virtual string Id { get; set; }

        [JsonProperty(JsonScheme._Rev)]
        public virtual string Rev { get; set; }

        [JsonProperty(JsonScheme.Conflicts)]
        public string[] Conflicts { get; set; }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Id: {2}{0}Rev: {3}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Id ?? NullValueForDebugOutput,
                Rev ?? NullValueForDebugOutput);
        }
    }
}