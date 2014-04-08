using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class CompactDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public CompactDatabaseHttpRequestFactory(IConnection connection, IRequestUrlGenerator requestUrlGenerator) : base(connection)
        {
            Ensure.That(requestUrlGenerator, "RequestUrlGenerator").IsNotNull();

            RequestUrlGenerator = requestUrlGenerator;
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