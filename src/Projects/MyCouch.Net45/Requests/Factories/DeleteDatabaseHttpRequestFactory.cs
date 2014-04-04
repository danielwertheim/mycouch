using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IDbRequestUrlGenerator DbRequestUrlGenerator { get; private set; }

        public DeleteDatabaseHttpRequestFactory(IConnection connection, IDbRequestUrlGenerator dbRequestUrlGenerator) : base(connection)
        {
            Ensure.That(dbRequestUrlGenerator, "dbRequestUrlGenerator").IsNotNull();

            DbRequestUrlGenerator = dbRequestUrlGenerator;
        }

        public virtual HttpRequest Create(DeleteDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<DeleteDatabaseRequest>(HttpMethod.Delete, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(DeleteDatabaseRequest request)
        {
            return DbRequestUrlGenerator.Generate(request.DbName);
        }
    }
}