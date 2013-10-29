namespace MyCouch.Cloudant.Requests.Configurators
{
    public class SearchIndexRequestConfigurator
    {
        protected readonly SearchIndexRequest Request;

        public SearchIndexRequestConfigurator(SearchIndexRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Lucene expression that will be used to query the index.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Expression(string value)
        {
            Request.Expression = value;

            return this;
        }

        /// <summary>
        /// Allow the results from a stale search index to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Stale(Stale value)
        {
            Request.Stale = value;

            return this;
        }

        /// <summary>
        /// A bookmark that was received from a previous search. This
        /// allows you to page through the results. If there are no more
        /// results after the bookmark, you will get a response with an
        /// empty rows array and the same bookmark. That way you can
        /// determine that you have reached the end of the result list.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Bookmark(string value)
        {
            Request.Bookmark = value;

            return this;
        }

        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        /// <param name="sortExpressions"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Sort(params string[] sortExpressions)
        {
            Request.Sort.Clear();
            Request.Sort.AddRange(sortExpressions);

            return this;
        }

        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator IncludeDocs(bool value)
        {
            Request.IncludeDocs = value;

            return this;
        }
        
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Limit(int value)
        {
            Request.Limit = value;

            return this;
        }
    }
}