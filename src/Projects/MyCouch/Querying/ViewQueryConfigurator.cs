namespace MyCouch.Querying
{
    public class ViewQueryConfigurator : IViewQueryConfigurator
    {
        protected readonly IViewQueryOptions Options;

        public ViewQueryConfigurator(IViewQueryOptions options)
        {
            Options = options;
        }

        public IViewQueryConfigurator Descending(bool value)
        {
            Options.Descending = value;

            return this;
        }

        public IViewQueryConfigurator Key(string value)
        {
            Options.Key = value;

            return this;
        }

        public IViewQueryConfigurator StartKey(string value)
        {
            Options.StartKey = value;

            return this;
        }

        public IViewQueryConfigurator StartKeyDocId(string value)
        {
            Options.StartKeyDocId = value;

            return this;
        }

        public IViewQueryConfigurator EndKey(string value)
        {
            Options.EndKey = value;

            return this;
        }

        public IViewQueryConfigurator EndKeyDocId(string value)
        {
            Options.EndKeyDocId = value;

            return this;
        }

        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IViewQueryConfigurator InclusiveEnd(bool value)
        {
            Options.InclusiveEnd = value;

            return this;
        }

        public IViewQueryConfigurator Skip(int value)
        {
            Options.Skip = value;

            return this;
        }

        public IViewQueryConfigurator Limit(int value)
        {
            Options.Limit = value;

            return this;
        }

        public IViewQueryConfigurator Reduce(bool value)
        {
            Options.Reduce = value;
            
            return this;
        }
    }
}