using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;

namespace MyCouch.Net
{
    public abstract class Connection : IConnection
    {
        protected HttpClient HttpClient { get; private set; }
        protected bool IsDisposed { get; private set; }

        public Uri Address
        {
            get { return HttpClient.BaseAddress; }
        }

        protected Connection(Uri dbUri)
        {
            Ensure.That(dbUri, "dbUri").IsNotNull();

            HttpClient = CreateHttpClient(dbUri);
            IsDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
                return;

            if (HttpClient != null)
            {
                HttpClient.CancelPendingRequests();
                HttpClient.Dispose();
                HttpClient = null;
            }
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private HttpClient CreateHttpClient(Uri dbUri)
        {
            var client = new HttpClient { BaseAddress = new Uri(dbUri.AbsoluteUri.TrimEnd('/')) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));

            if (!string.IsNullOrWhiteSpace(dbUri.UserInfo))
            {
                var parts = dbUri.UserInfo
                    .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => Uri.UnescapeDataString(p))
                    .ToArray();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", string.Join(":", parts).AsBase64Encoded());
            }

            return client;
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            ThrowIfDisposed();

            return await HttpClient.SendAsync(OnBeforeSend(httpRequest)).ForAwait();
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await HttpClient.SendAsync(OnBeforeSend(httpRequest), cancellationToken).ForAwait();
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            ThrowIfDisposed();

            return await HttpClient.SendAsync(OnBeforeSend(httpRequest), completionOption).ForAwait();
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await HttpClient.SendAsync(OnBeforeSend(httpRequest), completionOption, cancellationToken).ForAwait();
        }

        protected virtual HttpRequest OnBeforeSend(HttpRequest httpRequest)
        {
            ThrowIfDisposed();

            return httpRequest.RemoveRequestType();
        }
    }
}