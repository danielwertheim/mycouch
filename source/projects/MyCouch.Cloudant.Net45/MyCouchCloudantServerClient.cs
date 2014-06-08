using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantServerClient : IMyCouchCloudantServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerClientConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public ISecurity Security { get; private set; }
        public IDatabases Databases { get; private set; }
        public IReplicator Replicator { get; private set; }

        public MyCouchCloudantServerClient(string serverUrl) : this(new Uri(serverUrl)) { }

        public MyCouchCloudantServerClient(Uri serverUri) : this(new ServerClientConnection(serverUri)) { }

        public MyCouchCloudantServerClient(IServerClientConnection connection, MyCouchCloudantClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;

            bootstrapper = bootstrapper ?? new MyCouchCloudantClientBootstrapper();

            Serializer = bootstrapper.SerializerFn();
            Security = bootstrapper.SecurityFn(Connection);
            Databases = bootstrapper.DatabasesFn(Connection);
            Replicator = bootstrapper.ReplicatorFn(Connection);
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