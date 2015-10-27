using System;
using MyCouch.EnsureThat;
using MyCouch.Serialization;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantClient : IMyCouchCloudantClient
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
        public IViews Views { get; }
        public ISearches Searches { get; }
        public IQueries Queries { get; }

        public MyCouchCloudantClient(string serverAddress, string dbName, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new Uri(serverAddress), dbName, bootstrapper) { }

        public MyCouchCloudantClient(Uri serverAddress, string dbName, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new DbConnectionInfo(serverAddress, dbName), bootstrapper) { }

        public MyCouchCloudantClient(DbConnectionInfo connectionInfo, MyCouchCloudantClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            IsDisposed = false;
            bootstrapper = bootstrapper ?? MyCouchCloudantClientBootstrappers.Default;

            Connection = bootstrapper.DbConnectionFn(connectionInfo);
            Serializer = bootstrapper.SerializerFn();
            DocumentSerializer = bootstrapper.DocumentSerializerFn();
            Changes = bootstrapper.ChangesFn(Connection);
            Attachments = bootstrapper.AttachmentsFn(Connection);
            Database = bootstrapper.DatabaseFn(Connection);
            Documents = bootstrapper.DocumentsFn(Connection);
            Entities = bootstrapper.EntitiesFn(Connection);
            Views = bootstrapper.ViewsFn(Connection);
            Searches = bootstrapper.SearchesFn(Connection);
            Queries = bootstrapper.QueriesFn(Connection);
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