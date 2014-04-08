using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Net;

namespace MyCouch
{
    public interface IConnection : IDisposable
    {
        Uri Address { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest);
        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}