using EnsureThat;
using MyCouch.Cloudant.Querying.Selectors;
using System.Linq;

namespace MyCouch.Cloudant
{
    public class FindParametersConfigurator
    {
        protected readonly IFindParameters Parameters;

        public FindParametersConfigurator(IFindParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// JSON object describing criteria used to select documents.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator SelectorExpression(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.SelectorExpression = value;

            return this;
        }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Limit(int value)
        {
            Ensure.That(value, "value").IsGt(0);

            Parameters.Limit = value;

            return this;
        }

        /// <summary>
        /// Skip the first n results, where n is the value specified.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Skip(int value)
        {
            Parameters.Skip = value;

            return this;
        }

        /// <summary>
        /// List of fields with sort directions to specify sorting of results.
        /// </summary>
        /// <param name="sortExpressions"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Sort(params SortableField[] sortExpressions)
        {
            Ensure.That(sortExpressions, "sortExpressions").HasItems();

            Parameters.Sort = sortExpressions.ToList();

            return this;
        }

        /// <summary>
        /// The list of fields of the documents to be returned.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Fields(params string[] fields)
        {
            Ensure.That(fields, "fields").HasItems();

            Parameters.Fields = fields.ToList();

            return this;
        }

        /// <summary>
        /// Read quorum needed for the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator ReadQuorum(int value)
        {
            Ensure.That(value, "value").IsGte(1);

            Parameters.Limit = value;

            return this;
        }

        public virtual FindParametersConfigurator Selector(Selector value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.Selector = value;

            return this;
        }
    }
}
