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
        public IChanges Changes { get; private set; }
        public IAttachments Attachments { get; private set; }
        public IDatabases Databases { get; private set; }
        public IDocuments Documents { get; private set; }
        public IEntities Entities { get; protected set; }
        public IViews Views { get; private set; }

        public Client(string url) : this(new Uri(url)) { }

        public Client(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public Client(IConnection connection, ClientBootstraper bootstraper = null)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;

            bootstraper = bootstraper ?? new ClientBootstraper();

            Serializer = bootstraper.SerializerFn();
            Changes = bootstraper.ChangesFn(Connection);
            Attachments = bootstraper.AttachmentsFn(Connection);
            Databases = bootstraper.DatabasesFn(Connection);
            Documents = bootstraper.DocumentsFn(Connection);
            Entities = bootstraper.EntitiesFn(Connection);
            Views = bootstraper.ViewsFn(Connection);
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