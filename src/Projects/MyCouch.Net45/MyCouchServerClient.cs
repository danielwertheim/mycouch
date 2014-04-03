using System;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class MyCouchServerClient : IMyCouchServerClient
    {
        protected bool IsDisposed { get; private set; }

        public IConnection Connection { get; private set; }

        public MyCouchServerClient(string serverUrl) : this(new Uri(serverUrl)) { }

        public MyCouchServerClient(Uri serverUri) : this(new BasicHttpClientConnection(serverUri)) { }

        public MyCouchServerClient(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
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