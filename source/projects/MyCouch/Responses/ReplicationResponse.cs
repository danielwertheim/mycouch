﻿using System;
using Newtonsoft.Json;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class ReplicationResponse : Response,
        IDocumentHeader
    {
        [JsonProperty(JsonScheme.Id)]
        public string Id { get; set; }

        [JsonProperty(JsonScheme.Rev)]
        public string Rev { get; set; }
    }
}