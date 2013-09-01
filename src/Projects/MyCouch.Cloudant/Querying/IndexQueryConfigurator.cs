namespace MyCouch.Cloudant.Querying
{
    public class IndexQueryConfigurator
    {
        protected readonly IndexQueryOptions Options;

        public IndexQueryConfigurator(IndexQueryOptions options)
        {
            Options = options;
        }

        public virtual IndexQueryConfigurator Expression(string value)
        {
            Options.Expression = value;

            return this;
        }

        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IndexQueryConfigurator IncludeDocs(bool value)
        {
            Options.IncludeDocs = value;

            return this;
        }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IndexQueryConfigurator Descending(bool value)
        {
            Options.Descending = value;

            return this;
        }

        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IndexQueryConfigurator Skip(int value)
        {
            Options.Skip = value;

            return this;
        }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IndexQueryConfigurator Limit(int value)
        {
            Options.Limit = value;

            return this;
        }
    }
}