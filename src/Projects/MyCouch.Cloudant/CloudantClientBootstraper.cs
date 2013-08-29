using System;
using MyCouch.Cloudant.Contexts;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class CloudantClientBootstraper : ClientBootsraper
    {
        public Func<IConnection, ISearch> SearchFn { get; set; }

        public CloudantClientBootstraper()
        {
            ConfigureSearchFn();
        }

        private void ConfigureSearchFn()
        {
            SearchFn = cn => new Search(cn);
        }
    }
}