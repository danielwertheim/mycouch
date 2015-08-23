using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class DeleteDatabaseServerHttpRequestFactory : DeleteDatabaseHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(DeleteDatabaseRequest request)
        {
            return string.Format("/{0}", new UrlSegment(request.DbName));
        }
    }
}