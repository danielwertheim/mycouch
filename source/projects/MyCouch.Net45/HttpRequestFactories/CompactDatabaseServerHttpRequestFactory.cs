using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class CompactDatabaseServerHttpRequestFactory : CompactDatabaseHttpRequestFactory
    {
        protected override string GenerateRelativeUrl(CompactDatabaseRequest request)
        {
            return string.Format("/{0}/_compact", new UrlSegment(request.DbName));
        }
    }
}