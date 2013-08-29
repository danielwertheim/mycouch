using System;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class CloudantClient : Client, ICloudantClient
    {
        public ISearch Search { get; private set; }

        public CloudantClient(string url) 
            : base(new Uri(url)) { }

        public CloudantClient(Uri uri) 
            : base(new BasicHttpClientConnection(uri)) { }

        public CloudantClient(IConnection connection, CloudantClientBootstraper bootstraper = null)
            : base(connection, bootstraper ?? new CloudantClientBootstraper())
        {
            bootstraper = bootstraper ?? new CloudantClientBootstraper();

            Search = bootstraper.SearchFn(Connection);
        }
    }
}