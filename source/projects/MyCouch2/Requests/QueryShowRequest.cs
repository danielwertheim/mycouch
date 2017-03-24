using MyCouch.EnsureThat;
using MyCouch.Querying;
using System;
using System.Collections.Generic;

namespace MyCouch.Requests
{
    public class QueryShowRequest : Request, IShowParameters
    {
        protected IShowParameters State { get; private set; }
        /// <summary>
        /// Identitfies the show function that will be used for transformation.
        /// </summary>
        public ShowIdentity ShowIdentity
        {
            get { return State.ShowIdentity; }
        }

        /// <summary>
        /// The document id ofthe document to be transformed
        /// </summary>
        public string DocId
        {
            get { return State.DocId; }
            set { State.DocId = value; }
        }

        /// <summary>
        /// Used to set custom accept header values.
        /// Applicable e.g. when specifying a <see cref="ShowIdentity"/>
        /// that returns e.g. HTML.
        /// </summary>
        public string[] Accepts
        {
            get { return State.Accepts; }
            set { State.Accepts = value; }
        }

        /// <summary>
        /// Indicates if any <see cref="Accepts"/> has been specified.
        /// </summary>
        public bool HasAccepts
        {
            get { return State.HasAccepts; }
        }

        /// <summary>
        /// Additional custom query string parameters.
        /// </summary>
        public IDictionary<string, object> CustomQueryParameters
        {
            get { return State.CustomQueryParameters; }
            set { State.CustomQueryParameters = value; }
        }

        /// <summary>
        /// Indicates if there are any <see cref="CustomQueryParameters"/> or not.
        /// </summary>
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
