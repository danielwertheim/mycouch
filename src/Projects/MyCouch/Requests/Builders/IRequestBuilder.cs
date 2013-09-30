using System.Net.Http;
using MyCouch.Commands;

namespace MyCouch.Requests.Builders
{
    public interface IRequestBuilder<in T> where T : ICommand
    {
        HttpRequestMessage Create(T cmd);
    }
}