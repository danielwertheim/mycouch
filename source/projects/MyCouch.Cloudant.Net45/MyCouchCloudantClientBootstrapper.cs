using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClientBootstrapper : MyCouchClientBootstrapper
    {
        public Func<IServerClientConnection, ISecurity> SecurityFn { get; set; }
        public Func<IDbClientConnection, ISearches> SearchesFn { get; set; }

        public MyCouchCloudantClientBootstrapper()
        {
            ConfigureSecurityFn();
            ConfigureSearchesFn();
        }

        protected virtual void ConfigureSecurityFn()
        {
            SecurityFn = cn => new Security(
                cn,
                SerializerFn());
        }

        protected virtual void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(
                cn,
                DocumentSerializerFn());
        }
    }
}