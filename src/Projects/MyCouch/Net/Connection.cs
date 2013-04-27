using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnsureThat;

namespace MyCouch.Net
{
    public class Connection : IConnection
    {
        protected HttpClient HttpClient;

        public Uri Address
        {
            get { return HttpClient.BaseAddress; }
        }

        public Connection(Uri uri)
        {
            Ensure.That(uri, "uri").IsNotNull();

            HttpClient = CreateHttpClient(uri);
        }

        public virtual void Dispose()
        {
            if (HttpClient == null)
                throw new ObjectDisposedException(typeof(Connection).Name);

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