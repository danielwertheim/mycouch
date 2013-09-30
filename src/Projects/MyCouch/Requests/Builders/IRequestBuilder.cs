using System.Net.Http;

namespace MyCouch.Requests.Builders
{
    public interface IRequestBuilder<in T> where T : IRequest
    {
        HttpRequestMessage Create(T cmd);
    }
}