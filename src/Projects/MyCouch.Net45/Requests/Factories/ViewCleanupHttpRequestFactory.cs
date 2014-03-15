using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ViewCleanupHttpRequestFactory : HttpRequestFactoryBase
    {
        public ViewCleanupHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(ViewCleanupRequest request)
        {
            var httpRequest = CreateFor<ViewCleanupRequest>(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetJsonContent();

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl()
        {
            return string.Concat(Connection.Address, "/_view_cleanup");
        }
    }
}