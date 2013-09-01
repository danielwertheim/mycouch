using System;
using System.Collections.Generic;

namespace MyCouch.Cloudant.Querying
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class IndexQueryOptions
    {
        /// <summary>
        /// The Lucene expression that will be used to query the index.
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool IncludeDocs { get; set; }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool Descending { get; set; }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int Limit { get; set; }

        public IndexQueryOptions()
        {
            //Set defaults according to docs:
            //http://docs.couchdb.org/en/latest/api/database.html#get-db-all-docs
            //http://wiki.apache.org/couchdb/HTTP_view_API
            IncludeDocs = false;
            Descending = false;
        }

        public virtual IEnumerable<KeyValuePair<string, string>> ToKeyValues()
        {
            if(HasValue(Expression))
                yield return new KeyValuePair<string, string>("q", Expression);

            if (IncludeDocs)
                yield return new KeyValuePair<string, string>("include_docs", IncludeDocs.ToString().ToLower());

            if (Descending)
                yield return new KeyValuePair<string, string>("descending", Descending.ToString().ToLower());

            if (HasValue(Limit))
                yield return new KeyValuePair<string, string>("limit", Limit.ToString());

            if (HasValue(Skip))
                yield return new KeyValuePair<string, string>("skip", Skip.ToString());
        }

        protected virtual bool HasValue(string value)
        {
            return value != null;
        }

        protected virtual bool HasValue(int value)
        {
            return value > 0;
        }
    }
}