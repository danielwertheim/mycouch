using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IDbRequestUrlGenerator DbRequestUrlGenerator { get; private set; }

        public GetDatabaseHttpRequestFactory(IConnection connection, IDbRequestUrlGenerator dbRequestUrlGenerator) : base(connection)
        {
            Ensure.That(dbRequestUrlGenerator, "dbRequestUrlGenerator").IsNotNull();

            DbRequestUrlGenerator = dbRequestUrlGenerator;
        }

        public virtual HttpRequest Create(GetDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<GetDatabaseRequest>(HttpMethod.Get, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(GetDatabaseRequest request)
        {
            return DbRequestUrlGenerator.Generate(request.DbName);
        }
    }
}