using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetEntityHttpRequestFactory
    {
        public virtual HttpRequest Create(GetEntityRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.Rev);
        }

        protected virtual string GenerateRelativeUrl(GetEntityRequest request)
        {
            var urlParams = new UrlParams();
            urlParams.AddIfNotNullOrWhiteSpace("rev", request.Rev);

            return string.Format("/{0}{1}", new UrlSegment(request.Id), new QueryString(urlParams));
        }
    }
}