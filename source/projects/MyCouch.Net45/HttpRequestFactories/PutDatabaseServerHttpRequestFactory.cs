using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PutDatabaseServerHttpRequestFactory : PutDatabaseHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(PutDatabaseRequest request)
        {
            return string.Format("/{0}", new UrlSegment(request.DbName));
        }
    }
}