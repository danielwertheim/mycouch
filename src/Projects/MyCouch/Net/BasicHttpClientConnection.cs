using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnsureThat;

namespace MyCouch.Net
{
    public class BasicHttpClientConnection : IConnection
    {
        protected HttpClient HttpClient;

        public Uri Address
        {
            get { return HttpClient.BaseAddress; }
        }

        public BasicHttpClientConnection(Uri uri)
        {
            Ensure.That(uri, "uri").IsNotNull();

            HttpClient = CreateHttpClient(uri);
        }

        public virtual void Dispose()
        {
            if (HttpClient == null)
                throw new ObjectDisposedException(typeof(BasicHttpClientConnection).Name);

            HttpClient.CancelPendingRequests();
            HttpClient.Dispose();
            HttpClient = null;
        }

        protected virtual HttpClient CreateHttpClient(Uri uri)
        {
            var client = new HttpClient { BaseAddress = uri };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));

            return client;
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await HttpClient.SendAsync(request);
        }
    }
}