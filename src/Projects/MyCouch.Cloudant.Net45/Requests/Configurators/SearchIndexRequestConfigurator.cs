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
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Descending(bool value)
        {
            Request.Descending = value;

            return this;
        }
        
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchIndexRequestConfigurator Skip(int value)
        {
            Request.Skip = value;

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