using System;
using EnsureThat;
using MyCouch.Querying;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQuery : IViewQuery
    {
        public IViewIdentity View { get; private set; }
        public IViewQueryOptions Options { get; private set; }

        public ViewQuery(string designDocument, string viewName) : this(new ViewIdentity(designDocument, viewName)) { }

        public ViewQuery(IViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            View = viewIdentity;
            Options = new ViewQueryOptions();
        }

        public virtual IViewQuery Configure(Action<IViewQueryConfigurator> configurator)
        {
            configurator(new ViewQueryConfigurator(Options));

            return this;
        }
    }
}