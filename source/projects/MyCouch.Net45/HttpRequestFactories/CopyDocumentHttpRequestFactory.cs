using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class CopyDocumentHttpRequestFactory
    {
        public virtual HttpRequest Create(CopyDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(new HttpMethod("COPY"), GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.SrcRev);

            httpRequest.Headers.Add("Destination", request.NewId);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(CopyDocumentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddRequired("rev", request.SrcRev);

            return string.Format("/{0}{1}", new UrlSegment(request.SrcId), new QueryString(urlParams));
        }
    }
}