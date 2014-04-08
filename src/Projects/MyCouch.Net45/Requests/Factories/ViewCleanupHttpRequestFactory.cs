using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ViewCleanupHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public ViewCleanupHttpRequestFactory(IConnection connection, IRequestUrlGenerator requestUrlGenerator) : base(connection)
        {
            Ensure.That(requestUrlGenerator, "RequestUrlGenerator").IsNotNull();

            RequestUrlGenerator = requestUrlGenerator;
        }

        public virtual HttpRequest Create(ViewCleanupRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<ViewCleanupRequest>(HttpMethod.Post, GenerateRequestUrl(request));

            httpRequest.SetJsonContent();

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(ViewCleanupRequest request)
        {
            return string.Concat(RequestUrlGenerator.Generate(request.DbName), "/_view_cleanup");
        }
    }
}