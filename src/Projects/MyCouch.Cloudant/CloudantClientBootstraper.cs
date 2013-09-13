using System;
using MyCouch.Cloudant.Contexts;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class CloudantClientBootstraper : ClientBootsraper
    {
        public Func<IConnection, ISearches> SearchesFn { get; set; }

        public CloudantClientBootstraper()
        {
            ConfigureSearchesFn();
        }

        private void ConfigureSearchesFn()
        {
            SearchesFn = cn => new Searches(cn, EntitySerializationConfigurationFn());
        }
    }
}