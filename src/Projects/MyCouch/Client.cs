using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Schemes;
using MyCouch.Serialization;

namespace MyCouch
{
    public class Client : IClient
    {
        public IConnection Connection { get; protected set; }
        public ISerializer Serializer { get; set; }
        public IResponseFactory ResponseFactory { get; set; }
        public IEntityReflector EntityReflector { get; set; }
        public IDatabases Databases { get; protected set; }
        public IDocuments Documents { get; protected set; }
        public IViews Views { get; protected set; }

        public Client(string url) : this(new Uri(url)) { }

        public Client(Uri uri)
        {
            Ensure.That(uri, "uri").IsNotNull();

            Connection = new BasicHttpClientConnection(uri);
            EntityReflector = new EntityReflector();
            Serializer = new MyCouchSerializer(EntityReflector);
            ResponseFactory = new ResponseFactory(this);
            Databases = new Databases(this);
            Documents = new Documents(this);
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