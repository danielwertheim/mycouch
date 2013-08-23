using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch
{
    public class Client : IClient
    {
        protected readonly IResponseMaterializer ResponseMaterializer;

        public IConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IDatabases Databases { get; private set; }
        public IDocuments Documents { get; private set; }
        public IAttachments Attachments { get; private set; }
        public IViews Views { get; private set; }

        public Client(string url) : this(new Uri(url)) { }

        public Client(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public Client(IConnection connection, LightClientBootsraper bootstraper = null)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;

            bootstraper = bootstraper ?? new LightClientBootsraper();

            ResponseMaterializer = bootstraper.ResponseMaterializerResolver();
            Serializer = bootstraper.SerializerResolver();
            Attachments = bootstraper.AttachmentsResolver(Connection);
            Databases = bootstraper.DatabasesResolver(Connection);
            Documents = bootstraper.DocumentsResolver(Connection);
            Views = bootstraper.ViewsResolver(Connection);
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