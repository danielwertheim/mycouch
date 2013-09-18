using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCouch.Querying
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryOptions
    {
        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        public string Stale { get; set; }
        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool IncludeDocs { get; set; }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool Descending { get; set; }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        public string[] Keys { get; set; }
        /// <summary>
        /// Indicates if any <see cref="Keys"/> has been specified.
        /// </summary>
        public bool HasKeys
        {
            get { return Keys != null && Keys.Any(); }
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        public string StartKey { get; set; }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        public string StartKeyDocId { get; set; }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        public string EndKey { get; set; }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        public string EndKeyDocId { get; set; }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        public bool InclusiveEnd { get; set; }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        public bool Reduce { get; set; }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        public bool UpdateSeq { get; set; }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        public bool Group { get; set; }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        public int GroupLevel { get; set; }

        public ViewQueryOptions()
        {
            //Set defaults according to docs:
            //http://docs.couchdb.org/en/latest/api/database.html#get-db-all-docs
            //http://wiki.apache.org/couchdb/HTTP_view_API
            IncludeDocs = false;
            Descending = false;
            Reduce = true;
            InclusiveEnd = true;
            UpdateSeq = false;
            Group = false;
        }

        /// <summary>
        /// Returns Keys as compatible JSON document for use e.g.
        /// with POST of keys against views.
        /// </summary>
        /// <returns></returns>
        public virtual string GetKeysAsJson()
        {
            if (!HasKeys)
                return "{}";

            return string.Format("{{\"keys\":[{0}]}}",
                string.Join(",", Keys.Select(k => string.Format("\"{0}\"", k))));
        }

        public virtual IDictionary<string, string> ToKeyValues()
        {
            var kvs = new Dictionary<string, string>();

            if (IncludeDocs)
                kvs.Add(KeyValues.IncludeDocs, IncludeDocs.ToString().ToLower());

            if (Descending)
                kvs.Add(KeyValues.Descending, Descending.ToString().ToLower());

            if(!Reduce)
                kvs.Add(KeyValues.Reduce, Reduce.ToString().ToLower());

            if (!InclusiveEnd)
                kvs.Add(KeyValues.InclusiveEnd, InclusiveEnd.ToString().ToLower());

            if (UpdateSeq)
                kvs.Add(KeyValues.UpdateSeq, UpdateSeq.ToString().ToLower());

            if (Group)
                kvs.Add(KeyValues.Group, Group.ToString().ToLower());

            if (HasValue(GroupLevel))
                kvs.Add(KeyValues.GroupLevel, GroupLevel.ToString(MyCouchRuntime.NumberFormat));

            if (HasValue(Stale))
                kvs.Add(KeyValues.Stale, FormatValue(Stale));

            if (HasValue(Key))
                kvs.Add(KeyValues.Key, FormatValue(Key));

            if (HasValue(Keys))
                kvs.Add(KeyValues.Keys, FormatValue(Keys));

            if(HasValue(StartKey))
                kvs.Add(KeyValues.StartKey, FormatValue(StartKey));

            if (HasValue(StartKeyDocId))
                kvs.Add(KeyValues.StartKeyDocId, FormatValue(StartKeyDocId));
            
            if(HasValue(EndKey))
                kvs.Add(KeyValues.EndKey, FormatValue(EndKey));

            if (HasValue(EndKeyDocId))
                kvs.Add(KeyValues.EndKeyDocId, FormatValue(EndKeyDocId));
            
            if(HasValue(Limit))
                kvs.Add(KeyValues.Limit, Limit.ToString(MyCouchRuntime.NumberFormat));
            
            if(HasValue(Skip))
                kvs.Add(KeyValues.Skip, Skip.ToString(MyCouchRuntime.NumberFormat));

            return kvs;
        }

        protected virtual bool HasValue(string value)
        {
            return value != null;
        }

        protected virtual bool HasValue(IEnumerable<string> value)
        {
            return value != null && value.Any();
        }

        protected virtual bool HasValue(int value)
        {
            return value > 0;
        }

        protected virtual string FormatValue(string value)
        {
            return string.Format("\"{0}\"", value);
        }

        protected virtual string FormatValue(IEnumerable<string> value)
        {
            return string.Format("[{0}]", string.Join(",", value.Select(v => string.Format("\"{0}\"", v))));
        }

        public static class KeyValues
        {
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Reduce = "reduce";
            public const string InclusiveEnd = "inclusive_end";
            public const string UpdateSeq = "update_seq";
            public const string Group = "group";
            public const string GroupLevel = "group_level";
            public const string Stale = "stale";
            public const string Key = "key";
            public const string Keys = "keys";
            public const string StartKey = "startkey";
            public const string StartKeyDocId = "startkey_docid";
            public const string EndKey = "endkey";
            public const string EndKeyDocId = "endkey_docid";
            public const string Limit = "limit";
            public const string Skip = "skip";
        }
    }
}