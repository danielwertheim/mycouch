using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class CompactDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IDbRequestUrlGenerator DbRequestUrlGenerator { get; private set; }

        public CompactDatabaseHttpRequestFactory(IConnection connection, IDbRequestUrlGenerator dbRequestUrlGenerator) : base(connection)
        {
            Ensure.That(dbRequestUrlGenerator, "dbRequestUrlGenerator").IsNotNull();

            DbRequestUrlGenerator = dbRequestUrlGenerator;
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
            return string.Concat(DbRequestUrlGenerator.Generate(request.DbName), "/_compact");
        }
    }
}