using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PutDocumentHttpRequestFactory
    {
        public virtual HttpRequest Create(PutDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.Rev)
                .SetJsonContent(request.Content);
        }

        protected virtual string GenerateRelativeUrl(PutDocumentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddIfNotNullOrWhiteSpace("rev", request.Rev);
            urlParams.AddIfTrue("batch", request.Batch, "ok");

            return string.Format("/{0}{1}", new UrlSegment(request.Id), new QueryString(urlParams));
        }
    }
}