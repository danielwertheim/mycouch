using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace MyCouch.Querying
{
#if !PCL
    [Serializable]
#endif
    public class ListParameters : IListParameters
    {
        public ListIdentity ListIdentity { get; private set; }

        public ListParameters(ListIdentity listIdentity, string viewName)
        {
            Ensure.That(listIdentity, "listIdentity").IsNotNull();
            Ensure.That(viewName, "viewName").IsNotNullOrWhiteSpace();

            ListIdentity = listIdentity;
            ViewName = viewName;
        }

        public string ViewName { get; set; }
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
        public IDictionary<string, object> AdditionalQueryParameters { get; set; }

        public bool HasAdditionalQueryParameters
        {
            get { return AdditionalQueryParameters != null && AdditionalQueryParameters.Any(); }
        }
    }
}