using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PutDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public PutDatabaseHttpRequestFactory(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        public PutDatabaseHttpRequestFactory(IServerClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
        }

        public virtual HttpRequest Create(PutDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<PutDatabaseRequest>(HttpMethod.Put, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(PutDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate(request.DbName);
        }
    }
}