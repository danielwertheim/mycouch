using System;
using MyCouch.Net;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClient : MyCouchClient, IMyCouchCloudantClient
    {
        public ISearches Searches { get; private set; }

        public IQueries Queries { get; private set; }

        public MyCouchCloudantClient(string url)
            : this(new Uri(url)) { }

        public MyCouchCloudantClient(Uri uri)
            : this(new DbClientConnection(uri)) { }

        public MyCouchCloudantClient(IDbClientConnection connection, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : base(connection, bootstrapper ?? new MyCouchCloudantClientBootstrapper())
        {
            bootstrapper = bootstrapper ?? new MyCouchCloudantClientBootstrapper();

            Searches = bootstrapper.SearchesFn(Connection);
            Queries = bootstrapper.QueriesFn(Connection);
        }
    }
}