using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public abstract class HttpRequestFactoryBase
    {
        protected IConnection Connection { get; private set; }

        protected HttpRequestFactoryBase(IConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            Connection = connection;
        }

        protected virtual HttpRequest CreateFor<T>(HttpMethod method, string url) where T : Request
        {
            return new HttpRequest(method, url).SetRequestType(typeof(T));
        }
    }
}