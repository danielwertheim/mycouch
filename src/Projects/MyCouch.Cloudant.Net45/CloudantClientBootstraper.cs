using System;
using MyCouch.Cloudant.Contexts;

namespace MyCouch.Cloudant
{
    public class CloudantClientBootstraper : ClientBootstraper
    {
        public Func<IConnection, ISearches> SearchesFn { get; set; }

        public CloudantClientBootstraper()
        {
            ConfigureSearchesFn();
        }

        private void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(cn);
        }
    }
}