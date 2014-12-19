using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;
using System.Net.Http;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class DeleteIndexHttpRequestFactory
    {
        public virtual HttpRequest Create(DeleteIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Delete, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType());
        }

        protected virtual string GenerateRelativeUrl(DeleteIndexRequest request)
        {
            return string.Format("/_index/{0}/{1}/{2}",
                new UrlSegment(request.DesignDoc),
                new UrlSegment(request.Type.AsString()),
                new UrlSegment(request.Name));
        }
    }
}
