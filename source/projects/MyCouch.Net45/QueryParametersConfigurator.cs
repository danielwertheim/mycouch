using EnsureThat;

namespace MyCouch
{
    public class QueryParametersConfigurator
    {
        protected readonly IQueryParameters Parameters;

        public QueryParametersConfigurator(IQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Stale(Stale value)
        {
            Parameters.Stale = value;

            return this;
        }
        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator IncludeDocs(bool value)
        {
            Parameters.IncludeDocs = value;

            return this;
        }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Descending(bool value)
        {
            Parameters.Descending = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.Key = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.Key = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key(object[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.Key = value;

            return this;
        }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Keys(params object[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.Keys = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey(object[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKeyDocId(string value)
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
        public virtual QueryParametersConfigurator EndKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified complex-key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKey(object[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKeyDocId(string value)
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
        public virtual QueryParametersConfigurator InclusiveEnd(bool value)
        {
            Parameters.InclusiveEnd = value;

            return this;
        }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Skip(int value)
        {
            Parameters.Skip = value;

            return this;
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Limit(int value)
        {
            Parameters.Limit = value;

            return this;
        }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Reduce(bool value)
        {
            Parameters.Reduce = value;

            return this;
        }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator UpdateSeq(bool value)
        {
            Parameters.UpdateSeq = value;

            return this;
        }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Group(bool value)
        {
            Parameters.Group = value;

            return this;
        }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator GroupLevel(int value)
        {
            Parameters.GroupLevel = value;

            return this;
        }
    }
}