using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class HeadDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IDbRequestUrlGenerator DbRequestUrlGenerator { get; private set; }

        public HeadDatabaseHttpRequestFactory(IConnection connection, IDbRequestUrlGenerator dbRequestUrlGenerator)
            : base(connection)
        {
            Ensure.That(dbRequestUrlGenerator, "dbRequestUrlGenerator").IsNotNull();

            DbRequestUrlGenerator = dbRequestUrlGenerator;
        }

        public virtual HttpRequest Create(HeadDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<HeadDatabaseRequest>(HttpMethod.Head, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(HeadDatabaseRequest request)
        {
            return DbRequestUrlGenerator.Generate(request.DbName);
        }
    }
}