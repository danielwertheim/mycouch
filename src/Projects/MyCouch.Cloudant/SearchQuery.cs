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
        public SearchQueryOptions Options { get; private set; }

        public SearchQuery(string designDocument, string viewName)
            : this(new IndexIdentity(designDocument, viewName)) { }

        public SearchQuery(IndexIdentity index)
        {
            Ensure.That(index, "index").IsNotNull();

            Index = index;
            Options = new SearchQueryOptions();
        }

        public virtual SearchQuery Configure(Action<SearchQueryConfigurator> configurator)
        {
            configurator(new SearchQueryConfigurator(Options));

            return this;
        }
    }
}