using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class ReplicationResponse : Response
    {
        [JsonProperty(JsonScheme.NoChanges)]
        public bool NoChanges { get; set; }

        [JsonProperty(JsonScheme.SessionId)]
        public string SessionId { get; set; }

        [JsonProperty(JsonScheme.SourceLastSeq)]
        public string SourceLastSeq { get; set; }

        [JsonProperty(JsonScheme.ReplicationIdVersion)]
        public int ReplicationIdVersion { get; set; }

        [JsonProperty(JsonScheme.History)]
        public HistoryInfo[] History { get; set; }

        [JsonProperty(JsonScheme.LocalId)]
        public string LocalId { get; set; }

#if !PCL
        [Serializable]
#endif
        public class HistoryInfo
        {
            [JsonProperty(JsonScheme.SessionId)]
            public string SessionId { get; set; }

            [JsonProperty(JsonScheme.StartTime)]
            public DateTime StartTime { get; set; }

            [JsonProperty(JsonScheme.EndTime)]
            public DateTime EndTime { get; set; }

            [JsonProperty(JsonScheme.StartLastSeq)]
            public string StartLastSeq { get; set; }

            [JsonProperty(JsonScheme.EndLastSeq)]
            public string EndLastSeq { get; set; }

            [JsonProperty(JsonScheme.RecordedSeq)]
            public string RecordedSeq { get; set; }

            [JsonProperty(JsonScheme.MissingChecked)]
            public long MissingChecked { get; set; }

            [JsonProperty(JsonScheme.MissingFound)]
            public long MissingFound { get; set; }

            [JsonProperty(JsonScheme.DocsRead)]
            public long DocsRead { get; set; }

            [JsonProperty(JsonScheme.DocsWritten)]
            public long DocsWritten { get; set; }

            [JsonProperty(JsonScheme.DocWriteFailures)]
            public long DocWriteFailures { get; set; }
        }
    }
}