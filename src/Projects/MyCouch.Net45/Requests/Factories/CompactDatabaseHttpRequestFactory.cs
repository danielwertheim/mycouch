using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class CompactDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        public CompactDatabaseHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(CompactDatabaseRequest request)
        {
            var httpRequest = CreateFor<CompactDatabaseRequest>(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetJsonContent();

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl()
        {
            return string.Concat(Connection.Address, "/_compact");
        }
    }
}