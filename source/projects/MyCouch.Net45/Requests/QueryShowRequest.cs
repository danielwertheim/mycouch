using EnsureThat;
using MyCouch.Querying;
using System;
using System.Collections.Generic;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class QueryShowRequest : Request, IShowParameters
    {
        protected IShowParameters State { get; private set; }
        public ShowIdentity ShowIdentity
        {
            get { return State.ShowIdentity; }
        }
        public string DocId
        {
            get { return State.DocId; }
            set { State.DocId = value; }
        }
        public string[] Accepts
        {
            get { return State.Accepts; }
            set { State.Accepts = value; }
        }
        public bool HasAccepts
        {
            get { return State.HasAccepts; }
        }
        public IDictionary<string, object> CustomQueryParameters
        {
            get { return State.CustomQueryParameters; }
            set { State.CustomQueryParameters = value; }
        }
        public bool HasCustomQueryParameters
        {
            get { return State.HasCustomQueryParameters; }
        }
        public QueryShowRequest(string designDocument, string showName)
            : this(new ShowIdentity(designDocument, showName)) { }
        public QueryShowRequest(ShowIdentity showIdentity)
        {
            Ensure.That(showIdentity, "showIdentity").IsNotNull();

            State = new ShowParameters(showIdentity);
        }
        public virtual QueryShowRequest Configure(Action<QueryShowParametersConfigurator> configurator)
        {
            configurator(new QueryShowParametersConfigurator(State));

            return this;
        }
    }
}
