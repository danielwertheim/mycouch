using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PostDocumentHttpRequestFactory
    {
        public virtual HttpRequest Create(PostDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(request.Content);
        }

        protected virtual string GenerateRelativeUrl(PostDocumentRequest request)
        {
            var urlParams = new UrlParams();

            urlParams.AddIfTrue("batch", request.Batch, "ok");

            return string.Format("/{0}", new QueryString(urlParams));
        }
    }
}