using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Contexts
{
    public abstract class ApiContextBase<TConnection> where TConnection : class, IConnection
    {
        protected TConnection Connection { get; }

        protected ApiContextBase(TConnection connection)
        {
            Ensure.Any.IsNotNull(connection, nameof(connection));

            Connection = connection;
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            return Connection.SendAsync(httpRequest);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            return Connection.SendAsync(httpRequest, cancellationToken);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            return Connection.SendAsync(httpRequest, completionOption);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return Connection.SendAsync(httpRequest, completionOption, cancellationToken);
        }
    }
}