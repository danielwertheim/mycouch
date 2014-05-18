using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetAttachmentHttpRequestFactory
    {
        public virtual HttpRequest Create(GetAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.DocRev);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(GetAttachmentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddIfNotNullOrWhiteSpace("rev", request.DocRev);

            return string.Format("/{0}/{1}{2}",
                new UrlSegment(request.DocId),
                new UrlSegment(request.Name),
                new QueryString(urlParams));
        }
    }
}
