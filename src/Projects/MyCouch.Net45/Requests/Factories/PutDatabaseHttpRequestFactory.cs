using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PutDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public PutDatabaseHttpRequestFactory(IConnection connection, IRequestUrlGenerator requestUrlGenerator)
            : base(connection)
        {
            Ensure.That(requestUrlGenerator, "RequestUrlGenerator").IsNotNull();

            RequestUrlGenerator = requestUrlGenerator;
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