using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ViewCleanupHttpRequestFactory : DatabaseHttpRequestFactoryBase
    {
        public ViewCleanupHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(ViewCleanupRequest request)
        {
            var httpRequest = CreateFor<ViewCleanupRequest>(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetJsonContent();

            return httpRequest;
        }

        protected override string GenerateRequestUrl()
        {
            return string.Concat(base.GenerateRequestUrl(), "/_view_cleanup");
        }
    }
}