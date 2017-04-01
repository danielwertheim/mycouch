using System;
using EnsureThat;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchClient : IMyCouchClient
    {
        protected bool IsDisposed { get; private set; }

        public IDbConnection Connection { get; private set; }
        public ISerializer Serializer { get; }
        public ISerializer DocumentSerializer { get; }
        public IChanges Changes { get; }
        public IAttachments Attachments { get; }
        public IDatabase Database { get; }
        public IDocuments Documents { get; }
        public IEntities Entities { get; }
        public IQueries Queries { get; }
        //public ISearches Searches { get; }
        public IViews Views { get; }

        public MyCouchClient(string serverAddress, string dbName, MyCouchClientBootstrapper bootstrapper = null)
            : this(new Uri(serverAddress), dbName, bootstrapper) { }

        public MyCouchClient(Uri serverAddress, string dbName, MyCouchClientBootstrapper bootstrapper = null)
            : this(new DbConnectionInfo(serverAddress, dbName), bootstrapper) { }

        public MyCouchClient(DbConnectionInfo connectionInfo, MyCouchClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            IsDisposed = false;
            bootstrapper = bootstrapper ?? MyCouchClientBootstrappers.Default;

            Connection = bootstrapper.DbConnectionFn(connectionInfo);
            Serializer = bootstrapper.SerializerFn();
            DocumentSerializer = bootstrapper.DocumentSerializerFn();
            Changes = bootstrapper.ChangesFn(Connection);
            Attachments = bootstrapper.AttachmentsFn(Connection);
            Database = bootstrapper.DatabaseFn(Connection);
            Documents = bootstrapper.DocumentsFn(Connection);
            Entities = bootstrapper.EntitiesFn(Connection);
            Queries = bootstrapper.QueriesFn(Connection);
            //Searches = bootstrapper.SearchesFn(Connection);
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