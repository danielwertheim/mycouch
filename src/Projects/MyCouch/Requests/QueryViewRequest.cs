using System;
using EnsureThat;
using MyCouch.Querying;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class QueryViewRequest : IRequest
    {
        public ViewIdentity View { get; private set; }
        public ViewQueryOptions Options { get; private set; }

        public QueryViewRequest(string designDocument, string viewName)
            : this(new ViewIdentity(designDocument, viewName)) { }

        public QueryViewRequest(ViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            View = viewIdentity;
            Options = new ViewQueryOptions();
        }

        public virtual QueryViewRequest Configure(Action<ViewQueryConfigurator> configurator)
        {
            configurator(new ViewQueryConfigurator(Options));

            return this;
        }
    }
}