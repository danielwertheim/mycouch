using System.Collections.Generic;
using EnsureThat;

namespace MyCouch.Cloudant.Searching
{
    public class SearchParameters : ISearchParameters
    {
        public SearchIndexIdentity IndexIdentity { get; private set; }
        public string Expression { get; set; }
        public Stale? Stale { get; set; }
        public string Bookmark { get; set; }
        public IList<string> Sort { get; set; }
        public bool? IncludeDocs { get; set; }
        public int? Limit { get; set; }

        public SearchParameters(SearchIndexIdentity searchIndexIdentity)
        {
            Ensure.That(searchIndexIdentity, "searchIndexIdentity").IsNotNull();

            IndexIdentity = searchIndexIdentity;
            Sort = new List<string>();
        }
    }
}