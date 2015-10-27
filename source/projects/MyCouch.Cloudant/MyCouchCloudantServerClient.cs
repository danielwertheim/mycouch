using System;
using MyCouch.EnsureThat;
using MyCouch.Serialization;

namespace MyCouch.Cloudant
{
    public class MyCouchCloudantServerClient : IMyCouchCloudantServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerConnection Connection { get; private set; }
        public ISerializer Serializer { get; }
        public IDatabases Databases { get; }
        public IReplicator Replicator { get; }
        public ISecurity Security { get; }

        public MyCouchCloudantServerClient(string serverAddress, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new Uri(serverAddress), bootstrapper) { }

        public MyCouchCloudantServerClient(Uri serverAddress, MyCouchCloudantClientBootstrapper bootstrapper = null)
            : this(new ServerConnectionInfo(serverAddress), bootstrapper) { }

        public MyCouchCloudantServerClient(ServerConnectionInfo connectionInfo, MyCouchCloudantClientBootstrapper bootstrapper = null)
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