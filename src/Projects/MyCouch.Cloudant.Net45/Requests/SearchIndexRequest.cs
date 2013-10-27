using System;
using EnsureThat;
using MyCouch.Requests;
using MyCouch.Cloudant.Requests.Configurators;

namespace MyCouch.Cloudant.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchIndexRequest : Request
    {
        public SearchIndexIdentity IndexIdentity { get; private set; }

        /// <summary>
        /// The Lucene expression that will be used to query the index.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs { get; set; }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool? Descending { get; set; }

        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit { get; set; }

        public SearchIndexRequest(string designDocument, string viewName)
            : this(new SearchIndexIdentity(designDocument, viewName)) { }

        public SearchIndexRequest(SearchIndexIdentity indexIdentity)
        {
            Ensure.That(indexIdentity, "indexIdentity").IsNotNull();

            IndexIdentity = indexIdentity;
        }

        public virtual SearchIndexRequest Configure(Action<SearchIndexRequestConfigurator> configurator)
        {
            configurator(new SearchIndexRequestConfigurator(this));

            return this;
        }
    }
}