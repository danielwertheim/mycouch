using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.UnitTests.Fakes
{
    public class ConnectionFake : IConnection
    {
        public Uri Address { get; private set; }

        public ConnectionFake(Uri address)
        {
            Address = address;
        }

        public virtual void Dispose() { }
        
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, HttpCompletionOption completionOption)
        {
            return null;
        }
    }
}
