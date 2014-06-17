using System.Linq;
using EnsureThat;

namespace MyCouch.Cloudant
{
    public class SearchParametersConfigurator
    {
        protected readonly ISearchParameters Parameters;

        public SearchParametersConfigurator(ISearchParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Lucene expression that will be used to query the index.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchParametersConfigurator Expression(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.Expression = value;

            return this;
        }

        /// <summary>
        /// Allow the results from a stale search index to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchParametersConfigurator Stale(Stale value)
        {
            Parameters.Stale = value;

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
        public virtual SearchParametersConfigurator Bookmark(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.Bookmark = value;

            return this;
        }

        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        /// <param name="sortExpressions"></param>
        /// <returns></returns>
        public virtual SearchParametersConfigurator Sort(params string[] sortExpressions)
        {
            Ensure.That(sortExpressions, "sortExpressions").HasItems();

            Parameters.Sort = sortExpressions.ToList();

            return this;
        }

        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchParametersConfigurator IncludeDocs(bool value)
        {
            Parameters.IncludeDocs = value;

            return this;
        }
        
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SearchParametersConfigurator Limit(int value)
        {
            Parameters.Limit = value;

            return this;
        }
    }
}