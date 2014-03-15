using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClientBootstrapper : MyCouchClientBootstrapper
    {
        public Func<IConnection, ISearches> SearchesFn { get; set; }

        public MyCouchCloudantClientBootstrapper()
        {
            ConfigureSearchesFn();
        }

        protected virtual void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(
                cn,
                SerializerFn(),
                EntitySerializerFn());
        }
    }
}