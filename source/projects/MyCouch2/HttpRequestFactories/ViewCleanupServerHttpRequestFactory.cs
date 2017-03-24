using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class ViewCleanupServerHttpRequestFactory : ViewCleanupHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(ViewCleanupRequest request)
        {
            return string.Format("/{0}/_view_cleanup", new UrlSegment(request.DbName));
        }
    }
}