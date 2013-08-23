using System;
using MyCouch.Net;

namespace MyCouch.Rich
{
    public class RichClient : Client, IRichClient
    {
        public IEntities Entities { get; protected set; }

        public RichClient(string url) : this(new Uri(url)) { }

        public RichClient(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public RichClient(IConnection connection) : this(connection, new RichClientBootstraper()) { }

        public RichClient(IConnection connection, RichClientBootstraper bootstraper) : base(connection, bootstraper)
        {
            Entities = bootstraper.EntitiesResolver(Connection);
        }
    }
}