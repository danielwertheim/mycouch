using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Serialization;

namespace MyCouch
{
    public class MyCouchServerClient : IMyCouchServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IServerClientConnection Connection { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IDatabases Databases { get; private set; }

        public MyCouchServerClient(string serverUrl) : this(new Uri(serverUrl)) { }

        public MyCouchServerClient(Uri serverUri) : this(new ServerClientConnection(serverUri)) { }

        public MyCouchServerClient(IServerClientConnection connection, MyCouchServerClientBootstrapper bootstrapper = null)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;

            bootstrapper = bootstrapper ?? new MyCouchServerClientBootstrapper();

            Serializer = bootstrapper.SerializerFn();
            Databases = bootstrapper.DatabasesFn(Connection);
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