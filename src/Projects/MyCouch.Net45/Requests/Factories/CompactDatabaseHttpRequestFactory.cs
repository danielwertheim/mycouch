using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class CompactDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public CompactDatabaseHttpRequestFactory(IDbClientConnection connection)
            : base(connection)
        {
            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        public CompactDatabaseHttpRequestFactory(IServerClientConnection connection)
            : base(connection)
        {
            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
        }

        public virtual HttpRequest Create(CompactDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<CompactDatabaseRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            httpRequest.SetJsonContent();

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(CompactDatabaseRequest request)
        {
            return string.Concat(RequestUrlGenerator.Generate(request.DbName), "/_compact");
        }
    }
}