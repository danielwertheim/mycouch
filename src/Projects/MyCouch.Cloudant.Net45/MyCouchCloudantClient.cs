using System;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClient : MyCouchClient, IMyCouchCloudantClient
    {
        public ISearches Searches { get; private set; }

        public MyCouchCloudantClient(string url)
            : this(new Uri(url)) { }

        public MyCouchCloudantClient(Uri uri)
            : this(new BasicHttpClientConnection(uri)) { }

        public MyCouchCloudantClient(IConnection connection, MyCouchCloudantClientBootstraper bootstraper = null)
            : base(connection, bootstraper ?? new MyCouchCloudantClientBootstraper())
        {
            bootstraper = bootstraper ?? new MyCouchCloudantClientBootstraper();

            Searches = bootstraper.SearchesFn(Connection);
        }
    }
}