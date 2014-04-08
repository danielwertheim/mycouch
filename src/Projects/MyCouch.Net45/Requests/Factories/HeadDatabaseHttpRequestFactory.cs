using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class HeadDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public HeadDatabaseHttpRequestFactory(IConnection connection, IRequestUrlGenerator requestUrlGenerator)
            : base(connection)
        {
            Ensure.That(requestUrlGenerator, "RequestUrlGenerator").IsNotNull();

            RequestUrlGenerator = requestUrlGenerator;
        }

        public virtual HttpRequest Create(HeadDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<HeadDatabaseRequest>(HttpMethod.Head, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(HeadDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate(request.DbName);
        }
    }
}