namespace MyCouch.Responses
{
    public static class JsonScheme
    {
        public const string StartObject = "{";
        public const string EndObject = "}";
        public const string MemberDelimiter = ",";
        public const string MemberValueFormat = "\"{0}\":{1}";
        public const string MemberStringValueFormat = "\"{0}\":\"{1}\"";
        public const string MemberArrayValueFormat = "\"{0}\":[{1}]";
        public const string MemberObjectValueFormat = "\"{0}\":{1}";

        public const string Id = "id";
        public const string Rev = "rev";
        public const string Error = "error";
        public const string Reason = "reason";
        public const string UpdateSeq = "update_seq";
        public const string IncludedDoc = "doc";
        public const string LastSeq = "last_seq";
        public const string TotalRows = "total_rows";

        public const string LocalId = "_local_id";
        public const string NoChanges = "no_changes";
        public const string SessionId = "session_id";
        public const string SourceLastSeq = "source_last_seq";
        public const string ReplicationIdVersion = "replication_id_version";
        public const string History = "history";
        public const string StartTime = "start_time";
        public const string EndTime = "end_time";
        public const string StartLastSeq = "start_last_seq";
        public const string EndLastSeq = "end_last_seq";
        public const string RecordedSeq = "recorded_seq";
        public const string MissingChecked = "missing_checked";
        public const string MissingFound = "missing_found";
        public const string DocsRead = "docs_read";
        public const string DocsWritten = "docs_written";
        public const string DocWriteFailures = "doc_write_failures";
    }
}