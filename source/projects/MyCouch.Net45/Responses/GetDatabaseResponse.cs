using System;
using MyCouch.Serialization.Converters;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class GetDatabaseResponse : Response
    {
        [JsonProperty(JsonScheme.DbName)]
        public string DbName { get; set; }

        [JsonProperty(JsonScheme.DocCount)]
        public long DocCount { get; set; }

        [JsonProperty(JsonScheme.DocDelCount)]
        public long DocDelCount { get; set; }

        [JsonProperty(JsonScheme.UpdateSeq)]
        public string UpdateSeq { get; set; }

        [JsonProperty(JsonScheme.PurgeSeq)]
        public string PurgeSeq { get; set; }

        [JsonProperty(JsonScheme.CompactRunning)]
        public bool CompactRunning { get; set; }

        [JsonProperty(JsonScheme.DiskSize)]
        public long DiskSize { get; set; }

        [JsonProperty(JsonScheme.DataSize)]
        public long DataSize { get; set; }

        [JsonProperty(JsonScheme.InstanceStartTime)]
        [JsonConverter(typeof(UnixEpochDateTimeConverter))]
        public DateTime InstanceStartTimeUtc { get; set; }

        [JsonProperty(JsonScheme.DiskFormatVersion)]
        public int DiskFormatVersion { get; set; }

        [JsonProperty(JsonScheme.CommittedUpdateSeq)]
        public string CommittedUpdateSeq { get; set; }
    }
}