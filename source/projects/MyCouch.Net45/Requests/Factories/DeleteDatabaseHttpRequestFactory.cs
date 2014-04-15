using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public DeleteDatabaseHttpRequestFactory(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        public DeleteDatabaseHttpRequestFactory(IServerClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
        }

        public virtual HttpRequest Create(DeleteDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<DeleteDatabaseRequest>(HttpMethod.Delete, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(DeleteDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate(request.DbName);
        }
    }
}