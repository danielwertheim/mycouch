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

        public virtual IEnumerable<KeyValuePair<string, string>> ToKeyValues()
        {
            if (IncludeDocs)
                yield return new KeyValuePair<string, string>("include_docs", IncludeDocs.ToString().ToLower());

            if (Descending)
                yield return new KeyValuePair<string, string>("descending", Descending.ToString().ToLower());

            if(!Reduce)
                yield return new KeyValuePair<string, string>("reduce", Reduce.ToString().ToLower());

            if (!InclusiveEnd)
                yield return new KeyValuePair<string, string>("inclusive_end", InclusiveEnd.ToString().ToLower());

            if (UpdateSeq)
                yield return new KeyValuePair<string, string>("update_seq", UpdateSeq.ToString().ToLower());

            if (Group)
                yield return new KeyValuePair<string, string>("group", Group.ToString().ToLower());

            if (HasValue(GroupLevel))
                yield return new KeyValuePair<string, string>("group_level", GroupLevel.ToString());

            if (HasValue(Stale))
                yield return new KeyValuePair<string, string>("stale", FormatValue(Stale));

            if (HasValue(Key))
                yield return new KeyValuePair<string, string>("key", FormatValue(Key));

            if (HasValue(Keys))
                yield return new KeyValuePair<string, string>("keys", FormatValue(Keys));

            if(HasValue(StartKey))
                yield return new KeyValuePair<string, string>("startkey", FormatValue(StartKey));

            if (HasValue(StartKeyDocId))
                yield return new KeyValuePair<string, string>("startkey_docid", FormatValue(StartKeyDocId));
            
            if(HasValue(EndKey))
                yield return new KeyValuePair<string, string>("endkey", FormatValue(EndKey));

            if (HasValue(EndKeyDocId))
                yield return new KeyValuePair<string, string>("endkey_docid", FormatValue(EndKeyDocId));
            
            if(HasValue(Limit))
                yield return new KeyValuePair<string, string>("limit", Limit.ToString());
            
            if(HasValue(Skip))
                yield return new KeyValuePair<string, string>("skip", Skip.ToString());
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
    }
}