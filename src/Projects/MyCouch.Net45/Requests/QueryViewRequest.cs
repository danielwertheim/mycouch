using System;
using System.Linq;
using EnsureThat;
using MyCouch.Requests.Configurators;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class QueryViewRequest : Request
    {
        /// <summary>
        /// Identitfies the view that this Query request will be
        /// performed against.
        /// </summary>
        public ViewIdentity ViewIdentity { get; private set; }
        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        public Stale Stale { get; set; }
        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs { get; set; }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool? Descending { get; set; }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        public object Key { get; set; }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        public object[] Keys { get; set; }
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
        public object StartKey { get; set; }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        public string StartKeyDocId { get; set; }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        public object EndKey { get; set; }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        public string EndKeyDocId { get; set; }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        public bool? InclusiveEnd { get; set; }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int? Skip { get; set; }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit { get; set; }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        public bool? Reduce { get; set; }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        public bool? UpdateSeq { get; set; }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        public bool? Group { get; set; }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        public int? GroupLevel { get; set; }

        public QueryViewRequest(string designDocument, string viewName)
            : this(new ViewIdentity(designDocument, viewName)) { }

        public QueryViewRequest(ViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            ViewIdentity = viewIdentity;
        }

        public virtual QueryViewRequest Configure(Action<QueryViewRequestConfigurator> configurator)
        {
            configurator(new QueryViewRequestConfigurator(this));

            return this;
        }
    }
}