using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Net
{
    public interface IConnection : IDisposable
    {
        Uri Address { get; }
        HttpResponseMessage Send(HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}