using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Net
{
    public interface IConnection : IDisposable
    {
        Uri Address { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}