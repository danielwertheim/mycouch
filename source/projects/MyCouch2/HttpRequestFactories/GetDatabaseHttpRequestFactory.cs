using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetDatabaseHttpRequestFactory
    {
        public virtual HttpRequest Create(GetDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(GetDatabaseRequest request)
        {
            return "/";
        }
    }
}