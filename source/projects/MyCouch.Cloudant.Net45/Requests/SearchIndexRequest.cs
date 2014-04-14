using System;
using System.Collections.Generic;
using System.Linq;
using MyCouch.Cloudant.Searching;
using EnsureThat;
using MyCouch.Requests;

namespace MyCouch.Cloudant.Requests
{
#if !PCL
    [Serializable]
#endif
    public class SearchIndexRequest : Request, ISearchParameters
    {
        protected ISearchParameters State { get; private set; }

        /// <summary>
        /// Identitfies the Search index that this request will be
        /// performed against.
        /// </summary>
        public SearchIndexIdentity IndexIdentity { get { return State.IndexIdentity; } }

        /// <summary>
        /// The Lucene expression that will be used to query the index.
        /// </summary>
        public string Expression
        {
            get { return State.Expression; }
            set { State.Expression = value; }
        }

        /// <summary>
        /// Allow the results from a stale search index to be used.
        /// </summary>
        public Stale? Stale
        {
            get { return State.Stale; }
            set { State.Stale = value; }
        }

        /// <summary>
        /// A bookmark that was received from a previous search. This
        /// allows you to page through the results. If there are no more
        /// results after the bookmark, you will get a response with an
        /// empty rows array and the same bookmark. That way you can
        /// determine that you have reached the end of the result list.
        /// </summary>
        public string Bookmark
        {
            get { return State.Bookmark; }
            set { State.Bookmark = value; }
        }

        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        public IList<string> Sort
        {
            get { return State.Sort; }
            set { State.Sort = value; }
        }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs
        {
            get { return State.IncludeDocs; }
            set { State.IncludeDocs = value; }
        }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit
        {
            get { return State.Limit; }
            set { State.Limit = value; }
        }

        public SearchIndexRequest(string designDocument, string searchIndexName)
            : this(new SearchIndexIdentity(designDocument, searchIndexName)) { }

        public SearchIndexRequest(SearchIndexIdentity indexIdentity)
        {
            Ensure.That(indexIdentity, "indexIdentity").IsNotNull();

            State = new SearchParameters(indexIdentity);
        }

        public virtual SearchIndexRequest Configure(Action<SearchParametersConfigurator> configurator)
        {
            configurator(new SearchParametersConfigurator(State));

            return this;
        }

        public virtual bool HasSortings()
        {
            return Sort != null && Sort.Any();
        }
    }
}