using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace MyCouch.Querying
{
    public class QueryParameters : IQueryParameters
    {
        public ViewIdentity ViewIdentity { get; private set; }
        public string[] Accepts { get; set; }
        public bool HasAccepts
        {
            get { return Accepts != null && Accepts.Any(); }
        }
        public Stale? Stale { get; set; }
        public bool? IncludeDocs { get; set; }
        public bool? Descending { get; set; }
        public object Key { get; set; }
        public object[] Keys { get; set; }
        public bool HasKeys
        {
            get { return Keys != null && Keys.Any(); }
        }
        public object StartKey { get; set; }
        public string StartKeyDocId { get; set; }
        public object EndKey { get; set; }
        public string EndKeyDocId { get; set; }
        public bool? InclusiveEnd { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public bool? Reduce { get; set; }
        public bool? UpdateSeq { get; set; }
        public bool? Group { get; set; }
        public int? GroupLevel { get; set; }
        public string ListName { get; set; }
        public IDictionary<string, object> CustomQueryParameters { get; set; }
        public bool HasCustomQueryParameters
        {
            get { return CustomQueryParameters != null && CustomQueryParameters.Any(); }
        }

        public QueryParameters(ViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            ViewIdentity = viewIdentity;
        }
    }
}