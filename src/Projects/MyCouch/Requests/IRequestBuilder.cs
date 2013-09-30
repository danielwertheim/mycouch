using System.Net.Http;
using MyCouch.Commands;

namespace MyCouch.Requests
{
    public interface IRequestBuilder<in T> where T : ICommand
    {
        HttpRequestMessage Create(T cmd);
    }
}