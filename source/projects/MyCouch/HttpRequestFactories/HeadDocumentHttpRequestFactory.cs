using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class HeadDocumentHttpRequestFactory
    {
        public virtual HttpRequest Create(HeadDocumentRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Head, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.Rev);
        }

        protected virtual string GenerateRelativeUrl(HeadDocumentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddIfNotNullOrWhiteSpace("rev", request.Rev);

            return string.Format("/{0}{1}", new UrlSegment(request.Id), new QueryString(urlParams));
        }
    }
}