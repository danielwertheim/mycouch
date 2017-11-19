using System;
using EnsureThat;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchServerClient : IMyCouchServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerConnection Connection { get; private set; }
        public ISerializer Serializer { get; }
        public IDatabases Databases { get; }
        public IReplicator Replicator { get; }

        public MyCouchServerClient(string serverAddress, MyCouchClientBootstrapper bootstrapper = null)
            : this(new Uri(serverAddress), bootstrapper) { }

        public MyCouchServerClient(Uri serverAddress, MyCouchClientBootstrapper bootstrapper = null)
            : this(new ServerConnectionInfo(serverAddress), bootstrapper) { }

        public MyCouchServerClient(ServerConnectionInfo connectionInfo, MyCouchClientBootstrapper bootstrapper = null)
        {
            EnsureArg.IsNotNull(connectionInfo, nameof(connectionInfo));

            IsDisposed = false;
            bootstrapper = bootstrapper ?? MyCouchClientBootstrappers.Default;

            Connection = bootstrapper.ServerConnectionFn(connectionInfo);
            Serializer = bootstrapper.SerializerFn();
            Databases = bootstrapper.DatabasesFn(Connection);
            Replicator = bootstrapper.ReplicatorFn(Connection);
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