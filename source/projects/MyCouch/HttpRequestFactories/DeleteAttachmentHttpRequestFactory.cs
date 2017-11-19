using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class DeleteAttachmentHttpRequestFactory
    {
        public virtual HttpRequest Create(DeleteAttachmentRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = new HttpRequest(HttpMethod.Delete, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.DocRev);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(DeleteAttachmentRequest request)
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