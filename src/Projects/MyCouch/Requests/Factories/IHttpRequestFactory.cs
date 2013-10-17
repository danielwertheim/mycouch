using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public interface IHttpRequestFactory<in T> where T : Request
    {
        HttpRequest Create(T request);
    }
}