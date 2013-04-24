namespace MyCouch.Querying
{
    public class ViewQueryConfigurator : IViewQueryConfigurator
    {
        protected readonly IViewQueryOptions Options;

        public ViewQueryConfigurator(IViewQueryOptions options)
        {
            Options = options;
        }

        public virtual IViewQueryConfigurator IncludeDocs(bool value)
        {
            Options.IncludeDocs = value;

            return this;
        }

        public virtual IViewQueryConfigurator Descending(bool value)
        {
            Options.Descending = value;

            return this;
        }

        public virtual IViewQueryConfigurator Key(string value)
        {
            Options.Key = value;

            return this;
        }

        public virtual IViewQueryConfigurator StartKey(string value)
        {
            Options.StartKey = value;

            return this;
        }

        public virtual IViewQueryConfigurator StartKeyDocId(string value)
        {
            Options.StartKeyDocId = value;

            return this;
        }

        public virtual IViewQueryConfigurator EndKey(string value)
        {
            Options.EndKey = value;

            return this;
        }

        public virtual IViewQueryConfigurator EndKeyDocId(string value)
        {
            Options.EndKeyDocId = value;

            return this;
        }

        public virtual IViewQueryConfigurator InclusiveEnd(bool value)
        {
            Options.InclusiveEnd = value;

            return this;
        }

        public virtual IViewQueryConfigurator Skip(int value)
        {
            Options.Skip = value;

            return this;
        }

        public virtual IViewQueryConfigurator Limit(int value)
        {
            Options.Limit = value;

            return this;
        }

        public virtual IViewQueryConfigurator Reduce(bool value)
        {
            Options.Reduce = value;
            
            return this;
        }
    }
}