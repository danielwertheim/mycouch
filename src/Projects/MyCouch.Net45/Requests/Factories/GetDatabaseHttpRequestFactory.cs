using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        public GetDatabaseHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(GetDatabaseRequest request)
        {
            var httpRequest = CreateFor<GetDatabaseRequest>(HttpMethod.Get, GenerateRequestUrl());

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl()
        {
            return Connection.Address.ToString();
        }
    }
}