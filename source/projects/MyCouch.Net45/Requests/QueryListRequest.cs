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
        public object Key
        {
            get { return State.Key; }
            set { State.Key = value; }
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