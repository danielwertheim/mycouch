using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class ReplaceDocumentHttpRequestFactory
    {
        public virtual HttpRequest Create(ReplaceDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = new HttpRequest(new HttpMethod("COPY"), GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.SrcRev);

            httpRequest.Headers.Add("Destination", string.Concat(Uri.EscapeDataString(request.TrgId), "?rev=", request.TrgRev));

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(ReplaceDocumentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddRequired("rev", request.SrcRev);

            return string.Format("/{0}{1}", new UrlSegment(request.SrcId), new QueryString(urlParams));
        }
    }
}