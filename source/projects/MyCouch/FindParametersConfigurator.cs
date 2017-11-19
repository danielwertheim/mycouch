using System.Linq;
using EnsureThat;

namespace MyCouch
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
        /// <param name="formattingArgs"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator SelectorExpression(string value, params object[] formattingArgs)
        {
            Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

            Parameters.SelectorExpression = formattingArgs != null && formattingArgs.Any()
                ? string.Format(value, formattingArgs)
                : value;

            return this;
        }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Limit(int value)
        {
            EnsureArg.IsGt(value, 0, nameof(value));

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
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Sort(params SortableField[] fields)
        {
            EnsureArg.HasItems(fields, nameof(fields));

            Parameters.Sort = fields.ToList();

            return this;
        }

        /// <summary>
        /// List of fields with sort directions to specify sorting of results.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Sort(params string[] fields)
        {
            EnsureArg.HasItems(fields, nameof(fields));

            Parameters.Sort = fields.Select(f => new SortableField(f)).ToList();

            return this;
        }

        /// <summary>
        /// The list of fields of the documents to be returned.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator Fields(params string[] fields)
        {
            EnsureArg.HasItems(fields, nameof(fields));

            Parameters.Fields = fields.ToList();

            return this;
        }

        /// <summary>
        /// Read quorum needed for the result.
        /// Defaults to 1.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual FindParametersConfigurator ReadQuorum(int value)
        {
            EnsureArg.IsGte(value, 1, nameof(value));

            Parameters.ReadQuorum = value;

            return this;
        }
    }
}
