using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyCouch
{
    public interface IConnection : IDisposable
    {
        Uri Address { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}