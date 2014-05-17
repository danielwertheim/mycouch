using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class HeadDatabaseServerHttpRequestFactory : HeadDatabaseHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(HeadDatabaseRequest request)
        {
            return string.Format("/{0}", new UrlSegment(request.DbName));
        }
    }
}