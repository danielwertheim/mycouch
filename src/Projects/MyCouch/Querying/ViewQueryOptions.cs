using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCouch.Querying
{
    [Serializable]
    public class ViewQueryOptions : IViewQueryOptions
    {
        public bool Descending { get; set; }
        public string Key { get; set; }
        public string StartKey { get; set; }
        public string StartKeyDocId { get; set; }
        public string EndKey { get; set; }
        public string EndKeyDocId { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public bool Reduce { get; set; }

        public ViewQueryOptions()
        {
            //Set defaults according to docs: http://docs.couchdb.org/en/latest/api/database.html#get-db-all-docs
            Descending = false;
            Reduce = true;
        }

        public virtual IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ToKeyValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValues()
        {
            if (Descending)
                yield return new KeyValuePair<string, string>("descending", Descending.ToString().ToLower());

            if(!Reduce)
                yield return new KeyValuePair<string, string>("reduce", Reduce.ToString().ToLower());

            if (HasValue(Key))
                yield return new KeyValuePair<string, string>("key", FormatValue(Key));

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

        private static bool HasValue(string value)
        {
            return value != null;
        }

        private static bool HasValue(int value)
        {
            return value > 0;
        }

        private static string FormatValue(string value)
        {
            return Uri.EscapeDataString(value);
        }
    }
}