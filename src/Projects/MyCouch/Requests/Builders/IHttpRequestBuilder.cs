using MyCouch.Net;

namespace MyCouch.Requests.Builders
{
    public interface IHttpRequestBuilder<in T> where T : IRequest
    {
        HttpRequest Create(T request);
    }
}