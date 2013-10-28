using System;
using System.Collections.Generic;
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
        /// Allow the results from a stale search index to be used.
        /// </summary>
        public Stale? Stale { get; set; }

        /// <summary>
        /// A bookmark that was received from a previous search. This
        /// allows you to page through the results. If there are no more
        /// results after the bookmark, you will get a response with an
        /// empty rows array and the same bookmark. That way you can
        /// determine that you have reached the end of the result list.
        /// </summary>
        public string Bookmark { get; set; }
        
        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        public List<string> Sort { get; set; }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs { get; set; }

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
            Sort = new List<string>();
        }

        public virtual SearchIndexRequest Configure(Action<SearchIndexRequestConfigurator> configurator)
        {
            configurator(new SearchIndexRequestConfigurator(this));

            return this;
        }
    }
}