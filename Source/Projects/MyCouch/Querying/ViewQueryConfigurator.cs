namespace MyCouch.Querying
{
    public class ViewQueryConfigurator : IViewQueryConfigurator
    {
        protected readonly IViewQueryOptions Options;

        public ViewQueryConfigurator(IViewQueryOptions options)
        {
            Options = options;
        }

        public IViewQueryConfigurator StartKey(string value)
        {
            Options.StartKey = value;

            return this;
        }

        public IViewQueryConfigurator EndKey(string value)
        {
            Options.EndKey = value;

            return this;
        }

        public IViewQueryConfigurator Limit(int value)
        {
            Options.Limit = value;

            return this;
        }

        public IViewQueryConfigurator Skip(int value)
        {
            Options.Skip = value;

            return this;
        }

        public IViewQueryConfigurator Reduce(bool value)
        {
            Options.Reduce = value;
            
            return this;
        }
    }
}