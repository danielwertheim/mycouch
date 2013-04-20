using System;
using EnsureThat;

namespace MyCouch.Querying
{
    [Serializable]
    public class ViewQuery : IViewQuery
    {
        protected readonly IClient Client;

        public string DesignDocument { get; private set; }
        public string ViewName { get; private set; }
        public IViewQueryOptions Options { get; private set; }

        public ViewQuery(IClient client, string designDocument, string viewName)
        {
            Ensure.That(client, "Client").IsNotNull();
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(viewName, "viewName").IsNotNullOrWhiteSpace();

            Client = client;
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