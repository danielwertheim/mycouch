using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class EntityResponse<T> : ContentResponse<T>,
        IDocumentHeader
        where T : class
    {
        [JsonProperty(JsonScheme.Id)]
        public virtual string Id { get; set; }

        [JsonProperty(JsonScheme.Rev)]
        public virtual string Rev { get; set; }

        public override bool IsEmpty
        {
            get { return Content == null; }
        }

        public override string ToStringDebugVersion()
        {
            return string.Format("{1}{0}Content: {2}{0}Model: {3}{0}Id: {4}{0}Rev: {5}",
                Environment.NewLine,
                base.ToStringDebugVersion(),
                Content == null ? NullValueForDebugOutput : "<ENTITY>",
                typeof(T).Name,
                Id ?? NullValueForDebugOutput,
                Rev ?? NullValueForDebugOutput);
        }
    }
}