using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
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

        [JsonProperty(JsonScheme.Sizes)]
        public DbSizes Sizes { get; set; }

        [Obsolete("Use sizes.file instead")]
        [JsonProperty("disk_size")]
        public long DiskSize { get; set; }

        [Obsolete("Use sizes.active instead")]
        [JsonProperty("data_size")]
        public long DataSize { get; set; }

        [JsonProperty(JsonScheme.DiskFormatVersion)]
        public int DiskFormatVersion { get; set; }

        public class DbSizes
        {
            public long File { get; set; }
            public long External { get; set; }
            public long Active { get; set; }
        }
    }
}
