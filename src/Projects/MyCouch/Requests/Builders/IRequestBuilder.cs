using MyCouch.Net;

namespace MyCouch.Requests.Builders
{
    public interface IRequestBuilder<in T> where T : IRequest
    {
        HttpRequest Create(T request);
    }
}