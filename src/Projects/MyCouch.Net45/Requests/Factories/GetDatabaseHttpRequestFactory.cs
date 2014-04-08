using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetDatabaseHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public GetDatabaseHttpRequestFactory(IConnection connection, IRequestUrlGenerator requestUrlGenerator) : base(connection)
        {
            Ensure.That(requestUrlGenerator, "requestUrlGenerator").IsNotNull();

            RequestUrlGenerator = requestUrlGenerator;
        }

        public virtual HttpRequest Create(GetDatabaseRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<GetDatabaseRequest>(HttpMethod.Get, GenerateRequestUrl(request));

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(GetDatabaseRequest request)
        {
            return RequestUrlGenerator.Generate(request.DbName);
        }
    }
}