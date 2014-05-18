using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetDatabaseServerHttpRequestFactory : GetDatabaseHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(GetDatabaseRequest request)
        {
            return string.Format("/{0}", new UrlSegment(request.DbName));
        }
    }
}