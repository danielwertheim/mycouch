using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClientBootstraper : MyCouchClientBootstraper
    {
        public Func<IConnection, ISearches> SearchesFn { get; set; }

        public MyCouchCloudantClientBootstraper()
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