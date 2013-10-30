using System;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class CloudantClient : Client, ICloudantClient
    {
        public ISearches Searches { get; private set; }

        public CloudantClient(string url)
            : this(new Uri(url)) { }

        public CloudantClient(Uri uri)
            : this(new BasicHttpClientConnection(uri)) { }

        public CloudantClient(IConnection connection, CloudantClientBootstraper bootstraper = null)
            : base(connection, bootstraper ?? new CloudantClientBootstraper())
        {
            bootstraper = bootstraper ?? new CloudantClientBootstraper();

            Searches = bootstraper.SearchesFn(Connection);
        }
    }
}