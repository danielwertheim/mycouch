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
        TimeSpan Timeout { get; }
        Action<HttpRequest> BeforeSend { set; }
        Action<HttpResponseMessage> AfterSend { set; }

        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken = default);
    }
}