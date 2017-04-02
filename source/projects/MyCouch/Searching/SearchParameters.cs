//using System;
//using System.Collections.Generic;
//using EnsureThat;

//namespace MyCouch.Searching
//{
//    public class SearchParameters : ISearchParameters
//    {
//        public SearchIndexIdentity IndexIdentity { get; private set; }
//        public string Expression { get; set; }
//        public Stale? Stale { get; set; }
//        public string Bookmark { get; set; }
//        public IList<string> Sort { get; set; }
//        public bool? IncludeDocs { get; set; }
//        public int? Limit { get; set; }
//        public object Ranges { get; set; }
//        public IList<string> Counts { get; set; }
//        public string GroupField { get; set; }
//        public int? GroupLimit { get; set; }
//        public IList<string> GroupSort { get; set; }
//        public KeyValuePair<string, string>? DrillDown { get; set; }

//        public SearchParameters(SearchIndexIdentity searchIndexIdentity)
//        {
//            Ensure.That(searchIndexIdentity, "searchIndexIdentity").IsNotNull();

//            IndexIdentity = searchIndexIdentity;
//            Sort = new List<string>();
//            Counts = new List<string>();
//            GroupSort = new List<string>();
//        }
//    }
//}