using System;
using EnsureThat;
using MyCouch.Querying;
using System.Collections.Generic;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class QueryListRequest : Request, IListParameters
    {
        protected IListParameters State { get; private set; }

        /// <summary>
        /// Identitfies the List function that this request will be
        /// performed against.
        /// </summary>
        public ListIdentity ListIdentity { get { return State.ListIdentity; } }
        public string ViewName
        {
            get { return State.ViewName; }
            set { State.ViewName = value; }
        }

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

        public IDictionary<string, object> AdditionalQueryParameters
        {
            get { return State.AdditionalQueryParameters; }
            set { State.AdditionalQueryParameters = value; }
        }

        public bool HasAdditionalQueryParameters { get { return State.HasAdditionalQueryParameters; } }

        public QueryListRequest(string designDocument, string functionName, string viewName)
            : this(new ListIdentity(designDocument, functionName), viewName) { }

        public QueryListRequest(ListIdentity listIdentity, string viewName)
        {
            Ensure.That(listIdentity, "listIdentity").IsNotNull();
            Ensure.That(viewName, "viewName").IsNotNullOrWhiteSpace();

            State = new ListParameters(listIdentity, viewName);
        }

        internal QueryListRequest(IListParameters listParameters)
        {
            Ensure.That(listParameters, "listParameters").IsNotNull();

            State = listParameters;
        }

        public virtual QueryListRequest Configure(Action<ListParametersConfigurator> configurator)
        {
            configurator(new ListParametersConfigurator(State));

            return this;
        }
    }
}