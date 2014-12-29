using System;
using EnsureThat;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchClient : IMyCouchClient
    {
        protected bool IsDisposed { get; private set; }

        public IDbConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public ISerializer DocumentSerializer { get; private set; }
        public IChanges Changes { get; private set; }
        public IAttachments Attachments { get; private set; }
        public IDatabase Database { get; private set; }
        public IDocuments Documents { get; private set; }
        public IEntities Entities { get; protected set; }
        public IViews Views { get; private set; }

        public MyCouchClient(string dbUri, string dbName = null, MyCouchClientBootstrapper bootstrapper = null)
            : this(new Uri(dbUri), dbName, bootstrapper) { }

        public MyCouchClient(Uri dbUri, string dbName = null, MyCouchClientBootstrapper bootstrapper = null)
            : this(new ConnectionInfo(dbUri, dbName), bootstrapper) { }

        public MyCouchClient(ConnectionInfo connectionInfo, MyCouchClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            IsDisposed = false;
            bootstrapper = bootstrapper ?? new MyCouchClientBootstrapper();

            Connection = bootstrapper.DbConnectionFn(connectionInfo);
            Serializer = bootstrapper.SerializerFn();
            DocumentSerializer = bootstrapper.DocumentSerializerFn();
            Changes = bootstrapper.ChangesFn(Connection);
            Attachments = bootstrapper.AttachmentsFn(Connection);
            Database = bootstrapper.DatabaseFn(Connection);
            Documents = bootstrapper.DocumentsFn(Connection);
            Entities = bootstrapper.EntitiesFn(Connection);
            Views = bootstrapper.ViewsFn(Connection);
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