using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PutAttachmentHttpRequestFactory
    {
        public virtual HttpRequest Create(PutAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.DocRev)
                .SetContent(request.Content, request.ContentType);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(PutAttachmentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddRequired("rev", request.DocRev);

            return string.Format("/{0}/{1}{2}",
                new UrlSegment(request.DocId),
                new UrlSegment(request.Name),
                new QueryString(urlParams));
        }
    }
}