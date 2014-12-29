using System.Collections.Generic;
using EnsureThat;

namespace MyCouch
{
    public class QueryViewParametersConfigurator
    {
        protected readonly IQueryParameters Parameters;

        public QueryViewParametersConfigurator(IQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Stale(Stale value)
        {
            Parameters.Stale = value;

            return this;
        }

        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator IncludeDocs(bool value)
        {
            Parameters.IncludeDocs = value;

            return this;
        }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Descending(bool value)
        {
            Parameters.Descending = value;

            return this;
        }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Key(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetKey(value);
        }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Key<T>(T value)
        {
            return SetKey(value);
        }

        /// <summary>
        /// Return only documents that match the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Key<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetKey(value);
        }

        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Keys<T>(params T[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.Keys = value as object[];

            return this;
        }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator StartKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator StartKey<T>(T value)
        {
            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator StartKey<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator StartKeyDocId(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.StartKeyDocId = value;

            return this;
        }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator EndKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator EndKey<T>(T value)
        {
            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified complex-key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator EndKey<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator EndKeyDocId(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.EndKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator InclusiveEnd(bool value)
        {
            Parameters.InclusiveEnd = value;

            return this;
        }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Skip(int value)
        {
            Parameters.Skip = value;

            return this;
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Limit(int value)
        {
            Parameters.Limit = value;

            return this;
        }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Reduce(bool value)
        {
            Parameters.Reduce = value;

            return this;
        }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator UpdateSeq(bool value)
        {
            Parameters.UpdateSeq = value;

            return this;
        }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator Group(bool value)
        {
            Parameters.Group = value;

            return this;
        }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator GroupLevel(int value)
        {
            Parameters.GroupLevel = value;

            return this;
        }

        /// <summary>
        /// Additional custom query string parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator CustomQueryParameters(IDictionary<string, object> parameters)
        {
            Ensure.That(parameters, "parameters").HasItems();

            Parameters.CustomQueryParameters = parameters;

            return this;
        }

        /// <summary>
        /// Specify if you want to target a specific list in the view.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public virtual QueryViewParametersConfigurator WithList(string name, string accept = null)
        {
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            Parameters.ListName = name;
            if (!string.IsNullOrWhiteSpace(accept))
                Parameters.Accepts = new[] { accept };

            return this;
        }

        protected virtual QueryViewParametersConfigurator SetKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.Key = value;

            return this;
        }

        protected virtual QueryViewParametersConfigurator SetStartKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.StartKey = value;

            return this;
        }

        protected virtual QueryViewParametersConfigurator SetEndKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.EndKey = value;

            return this;
        }
    }
}