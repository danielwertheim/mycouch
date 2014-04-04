using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PutDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IDbRequestUrlGenerator DbRequestUrlGenerator { get; private set; }

        public PutDatabaseHttpRequestFactory(IConnection connection, IDbRequestUrlGenerator dbRequestUrlGenerator)
            : base(connection)
        {
            Ensure.That(dbRequestUrlGenerator, "dbRequestUrlGenerator").IsNotNull();

            DbRequestUrlGenerator = dbRequestUrlGenerator;
        }

        public virtual HttpRequest Create(PutDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<PutDatabaseRequest>(HttpMethod.Put, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(PutDatabaseRequest request)
        {
            return DbRequestUrlGenerator.Generate(request.DbName);
        }
    }
}