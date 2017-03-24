using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
    public class DocumentHeaderResponse : Response,
        IDocumentHeader
    {
        [JsonProperty(JsonScheme.Id)]
        public string Id { get; set; }

        [JsonProperty(JsonScheme.Rev)]
        public string Rev { get; set; }

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