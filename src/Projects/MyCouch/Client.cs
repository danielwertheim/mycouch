using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch
{
    public class Client : IClient
    {
        public IConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        protected IResponseFactory ResponseFactory { get; private set; }
        public IDatabase Database { get; private set; }
        public IDocuments Documents { get; private set; }
        public IAttachments Attachments { get; private set; }
        public IViews Views { get; private set; }

        public Client(string url) : this(new Uri(url)) { }

        public Client(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public Client(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
            Serializer = new DefaultSerializer();
            ResponseFactory = new ResponseFactory(new ResponseMaterializer());
            Database = new Database(Connection, ResponseFactory);
            Documents = new Documents(Connection, ResponseFactory);
            Attachments = new Attachments(Connection, ResponseFactory);
            Views = new Views(Connection, ResponseFactory);
        }

        public virtual void Dispose()
        {
            if (Connection == null)
                throw new ObjectDisposedException(typeof(Client).Name);

            Connection.Dispose();
            Connection = null;
        }
    }
}