using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public abstract class HttpRequestFactoryBase
    {
        protected virtual HttpRequest CreateFor<T>(HttpMethod method, string url) where T : Request
        {
            return new HttpRequest(method, url).SetRequestType(typeof(T));
        }
    }
}