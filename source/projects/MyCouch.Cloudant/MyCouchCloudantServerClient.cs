using System;
using MyCouch.EnsureThat;
using MyCouch.Serialization;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantServerClient : IMyCouchCloudantServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IDatabases Databases { get; private set; }
        public IReplicator Replicator { get; private set; }
        public ISecurity Security { get; private set; }

        public MyCouchCloudantServerClient(string serverUrl, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new Uri(serverUrl), bootstrapper) { }

        public MyCouchCloudantServerClient(Uri serverUri, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new ConnectionInfo(serverUri), bootstrapper) { }

        public MyCouchCloudantServerClient(ConnectionInfo connectionInfo, MyCouchCloudantClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            IsDisposed = false;
            bootstrapper = bootstrapper ?? MyCouchCloudantClientBootstrappers.Default;

            Connection = bootstrapper.ServerConnectionFn(connectionInfo);
            Serializer = bootstrapper.SerializerFn();
            Databases = bootstrapper.DatabasesFn(Connection);
            Replicator = bootstrapper.ReplicatorFn(Connection);
            Security = bootstrapper.SecurityFn(Connection);
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