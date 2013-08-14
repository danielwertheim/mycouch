using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Schemes;
using MyCouch.Schemes.Reflections;
using MyCouch.Serialization;

namespace MyCouch
{
    public class Client : IClient
    {
        public IConnection Connection { get; protected set; }
        public ISerializer Serializer { get; set; }
        public IResponseFactory ResponseFactory { get; set; }
        public IEntityReflector EntityReflector { get; set; }
        public IDatabase Database { get; protected set; }
        public IDocuments Documents { get; protected set; }
        public IEntities Entities { get; protected set; }
        public IAttachments Attachments { get; protected set; }
        public IViews Views { get; protected set; }

        public Client(string url) : this(new Uri(url)) { }

        public Client(Uri uri) : this(new BasicHttpClientConnection(uri)) { }

        public Client(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
#if !WinRT
            EntityReflector = new EntityReflector(new IlDynamicPropertyFactory());
#else
            EntityReflector = new EntityReflector(new LambdaDynamicPropertyFactory());
#endif
            Serializer = new MyCouchSerializer(() => EntityReflector);
            ResponseFactory = new ResponseFactory(this);
            Database = new Database(this);
            Documents = new Documents(this);
            Entities = new Entities(this);
            Attachments = new Attachments(this);
            Views = new Views(this);
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