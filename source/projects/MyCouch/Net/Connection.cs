using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
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

        public TimeSpan Timeout
        {
            get { return HttpClient.Timeout; }
        }

        public Action<HttpRequest> BeforeSend { protected get; set; }
        public Action<HttpResponseMessage> AfterSend { protected get; set; }

        protected Connection(ConnectionInfo connectionInfo)
        {
            Ensure.That(connectionInfo, "connectionInfo").IsNotNull();

            HttpClient = CreateHttpClient(connectionInfo);
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

        protected HttpClient CreateHttpClient(ConnectionInfo connectionInfo)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            var client = new HttpClient(handler, true)
            {
                BaseAddress = new Uri(connectionInfo.GetAbsoluteAddressExceptUserInfo().TrimEnd(new[] { '/' }))
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));

            if (connectionInfo.Timeout.HasValue)
                client.Timeout = connectionInfo.Timeout.Value;

            var basicAuthString = connectionInfo.GetBasicAuthString();
            if (basicAuthString != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthString.Value);
            }

            return client;
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                var response = await HttpClient.SendAsync(message).ForAwait();

                if (ShouldFollowResponse(response))
                {
                    using (var followMessage = CreateHttpRequestMessage(httpRequest))
                    {
                        followMessage.RequestUri = response.Headers.Location;
                        return await HttpClient.SendAsync(followMessage).ForAwait();
                    }
                }

                OnAfterSend(response);

                return response;
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                var response = await HttpClient.SendAsync(message, cancellationToken).ForAwait();

                if (ShouldFollowResponse(response))
                {
                    using (var followMessage = CreateHttpRequestMessage(httpRequest))
                    {
                        followMessage.RequestUri = response.Headers.Location;
                        return await HttpClient.SendAsync(followMessage, cancellationToken).ForAwait();
                    }
                }

                OnAfterSend(response);

                return response;
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                var response = await HttpClient.SendAsync(message, completionOption).ForAwait();

                if (ShouldFollowResponse(response))
                {
                    using (var followMessage = CreateHttpRequestMessage(httpRequest))
                    {
                        followMessage.RequestUri = response.Headers.Location;
                        return await HttpClient.SendAsync(followMessage, completionOption).ForAwait();
                    }
                }

                OnAfterSend(response);

                return response;
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                var response = await HttpClient.SendAsync(message, completionOption, cancellationToken).ForAwait();

                if (ShouldFollowResponse(response))
                {
                    using (var followMessage = CreateHttpRequestMessage(httpRequest))
                    {
                        followMessage.RequestUri = response.Headers.Location;
                        return await HttpClient.SendAsync(followMessage, completionOption, cancellationToken).ForAwait();
                    }
                }

                OnAfterSend(response);

                return response;
            }
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
        {
            httpRequest.RemoveRequestTypeHeader();

            var message = new HttpRequestMessage(httpRequest.Method, GenerateRequestUri(httpRequest))
            {
                Content = httpRequest.Content,
            };

            foreach (var kv in httpRequest.Headers)
                message.Headers.TryAddWithoutValidation(kv.Key, kv.Value);

            return message;
        }

        protected virtual bool ShouldFollowResponse(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.MovedPermanently && response.Headers.Location != null;
        }

        protected virtual void OnBeforeSend(HttpRequest httpRequest)
        {
            if (BeforeSend != null)
                BeforeSend(httpRequest);
        }

        protected virtual void OnAfterSend(HttpResponseMessage httpResponse)
        {
            if (AfterSend != null)
                AfterSend(httpResponse);
        }

        protected virtual string GenerateRequestUri(HttpRequest httpRequest)
        {
            return string.Format("{0}/{1}", Address.ToString().TrimEnd('/'), httpRequest.RelativeUrl.TrimStart('/'));
        }
    }
}