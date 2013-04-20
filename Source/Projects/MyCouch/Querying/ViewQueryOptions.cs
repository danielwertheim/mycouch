using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCouch.Querying
{
    [Serializable]
    public class ViewQueryOptions : IViewQueryOptions
    {
        public string StartKey { get; set; }
        public string EndKey { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public bool Reduce { get; set; }

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
            yield return new KeyValuePair<string, string>("reduce", Reduce.ToString().ToLower());
            
            if(HasValue(StartKey))
                yield return new KeyValuePair<string, string>("startkey", StartKey);
            
            if(HasValue(EndKey))
                yield return new KeyValuePair<string, string>("endkey", EndKey);
            
            if(HasValue(Limit))
                yield return new KeyValuePair<string, string>("limit", Limit.ToString());
            
            if(HasValue(Skip))
                yield return new KeyValuePair<string, string>("offset", Skip.ToString());
        }

        private static bool HasValue(string value)
        {
            return value != null;
        }

        private static bool HasValue(int value)
        {
            return value > 0;
        }
    }
}