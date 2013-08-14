using System;
using EnsureThat;
using MyCouch.Querying;

namespace MyCouch
{
#if !WinRT
    [Serializable]
#endif
    public class ViewQuery : IViewQuery
    {
        public IViewQueryOptions Options { get; private set; }
        public string DesignDocument { get; private set; }
        public string ViewName { get; private set; }

        public ViewQuery(string designDocument, string viewName)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewName, "viewName").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            ViewName = viewName;
            Options = new ViewQueryOptions();
        }

        public virtual IViewQuery Configure(Action<IViewQueryConfigurator> configurator)
        {
            configurator(new ViewQueryConfigurator(Options));

            return this;
        }
    }
}