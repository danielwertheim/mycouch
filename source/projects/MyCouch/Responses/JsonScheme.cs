namespace MyCouch.Responses
{
    public static class JsonScheme
    {
        public const string _Id = "_id";
        public const string _Rev = "_rev";

        public const string Id = "id";
        public const string Rev = "rev";
        public const string Error = "error";
        public const string Reason = "reason";
        public const string UpdateSeq = "update_seq";
        public const string Offset = "offset";
        public const string Rows = "rows";
        public const string Key = "key";
        public const string Password = "password";
        public const string IncludedDoc = "doc";
        public const string LastSeq = "last_seq";
        public const string TotalRows = "total_rows";
        public const string Conflicts = "_conflicts";

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

        public const string DbName = "db_name";
        public const string DocCount = "doc_count";
        public const string DocDelCount = "doc_del_count";
        public const string PurgeSeq = "purge_seq";
        public const string CompactRunning = "compact_running";
        public const string DiskSize = "disk_size";
        public const string DataSize = "data_size";
        public const string DiskFormatVersion = "disk_format_version";
    }
}