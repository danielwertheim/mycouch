namespace MyCouch.Querying
{
    public class ViewQueryConfigurator
    {
        protected readonly ViewQueryOptions Options;

        public ViewQueryConfigurator(ViewQueryOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Stale(string value)
        {
            Options.Stale = value;

            return this;
        }
        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Stale(Stale value)
        {
            Options.Stale = value;

            return this;
        }
        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator IncludeDocs(bool value)
        {
            Options.IncludeDocs = value;

            return this;
        }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Descending(bool value)
        {
            Options.Descending = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Key<T>(T value)
        {
            Options.Key = value;

            return this;
        }
        /// <summary>
        /// Return only documents that match the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Key<T>(T[] value)
        {
            Options.Key = value;

            return this;
        }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Keys(params string[] value)
        {
            Options.Keys = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator StartKey<T>(T value)
        {
            Options.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator StartKey<T>(T[] value)
        {
            Options.StartKey = value;

            return this;
        }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator StartKeyDocId(string value)
        {
            Options.StartKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator EndKey(string value)
        {
            Options.EndKey = value;

            return this;
        }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator EndKeyDocId(string value)
        {
            Options.EndKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator InclusiveEnd(bool value)
        {
            Options.InclusiveEnd = value;

            return this;
        }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Skip(int value)
        {
            Options.Skip = value;

            return this;
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Limit(int value)
        {
            Options.Limit = value;

            return this;
        }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Reduce(bool value)
        {
            Options.Reduce = value;
            
            return this;
        }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator UpdateSeq(bool value)
        {
            Options.UpdateSeq = value;

            return this;
        }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator Group(bool value)
        {
            Options.Group = value;

            return this;
        }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ViewQueryConfigurator GroupLevel(int value)
        {
            Options.GroupLevel = value;

            return this;
        }
    }
}