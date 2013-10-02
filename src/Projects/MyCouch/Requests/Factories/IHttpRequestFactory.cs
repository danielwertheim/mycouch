using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public interface IHttpRequestFactory<in T> where T : IRequest
    {
        HttpRequest Create(T request);
    }
}