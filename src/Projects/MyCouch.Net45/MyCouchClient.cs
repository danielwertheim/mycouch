using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchClient : IMyCouchClient
    {
        protected bool IsDisposed { get; private set; }

        public IConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IChanges Changes { get; private set; }
        public IAttachments Attachments { get; private set; }
        public IDatabase Database { get; private set; }
        public IDocuments Documents { get; private set; }
        public IEntities Entities { get; protected set; }
        public IViews Views { get; private set; }

        public MyCouchClient(string dbUri) : this(new Uri(dbUri)) { }

        public MyCouchClient(Uri dbUri) : this(new BasicHttpClientConnection(dbUri)) { }

        public MyCouchClient(IConnection connection, MyCouchClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;

            bootstrapper = bootstrapper ?? new MyCouchClientBootstrapper();

            Serializer = bootstrapper.SerializerFn();
            Changes = bootstrapper.ChangesFn(Connection);
            Attachments = bootstrapper.AttachmentsFn(Connection);
            Database = bootstrapper.DatabasesFn(Connection);
            Documents = bootstrapper.DocumentsFn(Connection);
            Entities = bootstrapper.EntitiesFn(Connection);
            Views = bootstrapper.ViewsFn(Connection);
            IsDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
                return;

            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}