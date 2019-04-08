using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Net;

namespace UnitTests.Fakes
{
    public class ServerClientConnectionFake : IServerConnection
    {
        public Uri Address { get; }
        public TimeSpan Timeout { get; }
        public Action<HttpRequest> BeforeSend { set; private get; }
        public Action<HttpResponseMessage> AfterSend { set; private get; }

        public ServerClientConnectionFake(ConnectionInfo connectionInfo)
        {
            Address = connectionInfo.Address;
            Timeout = connectionInfo.Timeout ?? Timeout;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }

    public class DbClientConnectionFake : IDbConnection
    {
        public string DbName { get; }
        public Uri Address { get; }
        public TimeSpan Timeout { get; }
        public Action<HttpRequest> BeforeSend { set; private get; }
        public Action<HttpResponseMessage> AfterSend { set; private get; }

        public DbClientConnectionFake(DbConnectionInfo connectionInfo)
        {
            Address = connectionInfo.Address;
            DbName = connectionInfo.DbName;
            Timeout = connectionInfo.Timeout ?? Timeout;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
