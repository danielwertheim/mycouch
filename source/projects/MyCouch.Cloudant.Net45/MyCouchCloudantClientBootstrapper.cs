using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClientBootstrapper : MyCouchClientBootstrapper
    {
        public Func<IServerClientConnection, ISecurity> SecurityFn { get; set; }
        public Func<IDbClientConnection, ISearches> SearchesFn { get; set; }
        public Func<IDbClientConnection, IQueries> QueriesFn { get; set; }

        public MyCouchCloudantClientBootstrapper()
        {
            ConfigureSecurityFn();
            ConfigureSearchesFn();
            ConfigureQueriesFn();
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
                DocumentSerializerFn(),
                SerializerFn());
        }

        protected virtual void ConfigureQueriesFn()
        {
            QueriesFn = cn => new Queries(
                cn,
                DocumentSerializerFn(),
                SerializerFn());
        }
    }
}