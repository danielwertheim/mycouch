using System;
using EnsureThat;
using MyCouch.Cloudant.Querying;

namespace MyCouch.Cloudant
{
    /// <summary>
    /// Defines a Lucene query for an existing Cloudant compatible search-index.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchQuery
    {
        public IndexIdentity Index { get; private set; }
        public IndexQueryOptions Options { get; private set; }

        public SearchQuery(string designDocument, string viewName)
            : this(new IndexIdentity(designDocument, viewName)) { }

        public SearchQuery(IndexIdentity index)
        {
            Ensure.That(index, "index").IsNotNull();

            Index = index;
            Options = new IndexQueryOptions();
        }

        public virtual SearchQuery Configure(Action<IndexQueryConfigurator> configurator)
        {
            configurator(new IndexQueryConfigurator(Options));

            return this;
        }
    }
}