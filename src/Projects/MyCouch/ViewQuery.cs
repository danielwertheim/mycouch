using System;
using EnsureThat;
using MyCouch.Commands;
using MyCouch.Querying;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQuery : ICommand
    {
        public ViewIdentity View { get; private set; }
        public ViewQueryOptions Options { get; private set; }

        public ViewQuery(string designDocument, string viewName)
            : this(new ViewIdentity(designDocument, viewName)) { }

        public ViewQuery(ViewIdentity viewIdentity)
        {
            Ensure.That(viewIdentity, "viewIdentity").IsNotNull();

            View = viewIdentity;
            Options = new ViewQueryOptions();
        }

        public virtual ViewQuery Configure(Action<ViewQueryConfigurator> configurator)
        {
            configurator(new ViewQueryConfigurator(Options));

            return this;
        }
    }
}