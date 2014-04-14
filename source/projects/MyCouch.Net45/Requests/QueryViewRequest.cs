using System;
using EnsureThat;
using MyCouch.Querying;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class QueryViewRequest : Request, IQueryParameters
    {
        protected IQueryParameters State { get; private set; }

        /// <summary>
        /// Identitfies the View index that this request will be
        /// performed against.
        /// </summary>
        public ViewIdentity ViewIdentity { get { return State.ViewIdentity; } }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        public Stale? Stale
        {
            get { return State.Stale; }
            set { State.Stale = value; }
        }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs
        {
            get { return State.IncludeDocs; }
            set { State.IncludeDocs = value; }
        }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool? Descending
        {
            get { return State.Descending; }
            set { State.Descending = value; }
        }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        public object Key
        {
            get { return State.Key; }
            set { State.Key = value; }
        }

        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        public object[] Keys
        {
            get { return State.Keys; }
            set { State.Keys = value; }
        }

        /// <summary>
        /// Indicates if any <see cref="Keys"/> has been specified.
        /// </summary>
        public bool HasKeys
        {
            get { return State.HasKeys; }
        }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        public object StartKey
        {
            get { return State.StartKey; }
            set { State.StartKey = value; }
        }

        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        public string StartKeyDocId
        {
            get { return State.StartKeyDocId; }
            set { State.StartKeyDocId = value; }
        }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        public object EndKey
        {
            get { return State.EndKey; }
            set { State.EndKey = value; }
        }

        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        public string EndKeyDocId
        {
            get { return State.EndKeyDocId; }
            set { State.EndKeyDocId = value; }
        }

        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        public bool? InclusiveEnd
        {
            get { return State.InclusiveEnd; }
            set { State.InclusiveEnd = value; }
        }

        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int? Skip
        {
            get { return State.Skip; }
            set { State.Skip = value; }
        }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit
        {
            get { return State.Limit; }
            set { State.Limit = value; }
        }

        /// <summary>
        /// Use the reduction function.
        /// </summary>
        public bool? Reduce
        {
            get { return State.Reduce; }
            set { State.Reduce = value; }
        }

        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        public bool? UpdateSeq
        {
            get { return State.UpdateSeq; }
            set { State.UpdateSeq = value; }
        }

        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        public bool? Group
        {
            get { return State.Group; }
            set { State.Group = value; }
        }

        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        public int? GroupLevel
        {
            get { return State.GroupLevel; }
            set { State.GroupLevel = value; }
        }

        /// <summary>
        /// Only system views like all_docs could be
        /// queried without specifying a designDocument.
        /// If a custom view is being targetted, use other cTor.
        /// </summary>
        /// <param name="viewName"></param>
        public QueryViewRequest(string viewName)
            : this(new SystemViewIdentity(viewName)) { }
        
        public QueryViewRequest(string designDocument, string viewName)
            : this(new ViewIdentity(designDocument, viewName)) { }

        public QueryViewRequest(SystemViewIdentity systemViewIdentity)
        {
            Ensure.That(systemViewIdentity, "systemViewIdentity").IsNotNull();

            State = new QueryParameters(systemViewIdentity);
        }

        public QueryViewRequest(ViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            State = new QueryParameters(viewIdentity);
        }

        internal QueryViewRequest(IQueryParameters queryParameters)
        {
            Ensure.That(queryParameters, "queryParameters").IsNotNull();

            State = queryParameters;
        }

        public virtual QueryViewRequest Configure(Action<QueryParametersConfigurator> configurator)
        {
            configurator(new QueryParametersConfigurator(State));

            return this;
        }
    }
}