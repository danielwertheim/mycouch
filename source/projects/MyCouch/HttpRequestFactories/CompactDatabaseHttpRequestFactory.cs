using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class CompactDatabaseHttpRequestFactory
    {
        public virtual HttpRequest Create(CompactDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent();
        }

        protected virtual string GenerateRelativeUrl(CompactDatabaseRequest request)
        {
            return "/_compact";
        }
    }
}