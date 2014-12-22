
using EnsureThat;
using MyCouch.Querying;
using System;
using System.Collections.Generic;
namespace MyCouch.Requests
{
    public class ShowRequest : Request, IShowParameters
    {
        protected IShowParameters State { get; private set; }        

        /// <summary>
        /// Identitfies the Show function that this request will be
        /// performed against.
        public ShowIdentity ShowIdentity
        {
            get { return State.ShowIdentity; }
        }

        /// <summary>
        /// The id of the document to which the show function will be applied.
        /// </summary>
        public string Id
        {
            get { return State.Id; }
            set { State.Id = value; }
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

        internal ShowRequest(IShowParameters showParameters)
        {
            Ensure.That(showParameters, "showParameters").IsNotNull();

            State = showParameters;
        }

        public ShowRequest(string designDocument, string showName)
            : this(new ShowIdentity(designDocument, showName)) { }

        public ShowRequest(ShowIdentity showIdentity)
        {
            Ensure.That(showIdentity, "showIdentity").IsNotNull();

            State = new ShowParameters(showIdentity);
        }

        public virtual ShowRequest Configure(Action<ShowParametersConfigurator> configurator)
        {
            configurator(new ShowParametersConfigurator(State));

            return this;
        }
    }
}
