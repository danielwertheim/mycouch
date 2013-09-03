using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class JsonResponseMappings : Dictionary<string, Action<JsonReader>>
    {
        public static class FieldNames
        {
            public const string Id = "id";
            public const string Rev = "rev";
            public const string TotalRows = "total_rows";
            public const string UpdateSeq = "update_seq";
            public const string Offset = "offset";
            public const string Rows = "rows";
            public const string Error = "error";
            public const string Reason = "reason";
        }
    }
}