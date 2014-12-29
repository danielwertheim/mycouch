using System;
using EnsureThat;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchServerClient : IMyCouchServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IDatabases Databases { get; private set; }
        public IReplicator Replicator { get; private set; }

        public MyCouchServerClient(string serverUrl, MyCouchClientBootstrapper bootstrapper = null)
            : this(new Uri(serverUrl), bootstrapper) { }

        public MyCouchServerClient(Uri serverUri, MyCouchClientBootstrapper bootstrapper = null)
            : this(new ConnectionInfo(serverUri), bootstrapper) { }

        public MyCouchServerClient(ConnectionInfo connectionInfo, MyCouchClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            IsDisposed = false;
            bootstrapper = bootstrapper ?? new MyCouchClientBootstrapper();

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