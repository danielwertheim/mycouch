using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClientBootstrapper : MyCouchClientBootstrapper
    {
        public Func<IDbClientConnection, ISearches> SearchesFn { get; set; }

        public MyCouchCloudantClientBootstrapper()
        {
            ConfigureSearchesFn();
        }

        protected virtual void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(
                cn,
                EntitySerializerFn());
        }
    }
}