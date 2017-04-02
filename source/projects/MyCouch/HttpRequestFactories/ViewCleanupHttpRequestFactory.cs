using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class ViewCleanupHttpRequestFactory
    {
        public virtual HttpRequest Create(ViewCleanupRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent();
        }

        protected virtual string GenerateRelativeUrl(ViewCleanupRequest request)
        {
            return "/_view_cleanup";
        }
    }
}