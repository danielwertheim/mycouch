using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PutDatabaseHttpRequestFactory
    {
        public virtual HttpRequest Create(PutDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(PutDatabaseRequest request)
        {
            return "/";
        }
    }
}