using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Responses;
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

        public Client(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            ResponseMaterializer = new DefaultResponseMaterializer();

            Connection = connection;
            Serializer = new DefaultSerializer();
            Databases = new Databases(Connection, new DatabaseResponseFactory(ResponseMaterializer));
            Documents = new Documents(Connection, new DocumentResponseFactory(ResponseMaterializer), new DocumentHeaderResponseFactory(ResponseMaterializer), new BulkResponseFactory(ResponseMaterializer));
            Attachments = new Attachments(Connection, new AttachmentResponseFactory(ResponseMaterializer), new DocumentHeaderResponseFactory(ResponseMaterializer));
            Views = new Views(Connection, new JsonViewQueryResponseFactory(ResponseMaterializer), new ViewQueryResponseFactory(ResponseMaterializer));
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