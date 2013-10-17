using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;

namespace MyCouch.Contexts
{
    public abstract class ApiContextBase
    {
        protected readonly IConnection Connection;

        protected ApiContextBase(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest)
        {
            return Connection.SendAsync(httpRequest);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, HttpCompletionOption completionOption)
        {
            return Connection.SendAsync(httpRequest, completionOption);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return Connection.SendAsync(httpRequest, completionOption, cancellationToken);
        }
    }
}