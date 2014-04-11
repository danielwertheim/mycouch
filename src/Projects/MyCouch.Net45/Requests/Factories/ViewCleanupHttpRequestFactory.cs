using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ViewCleanupHttpRequestFactory : HttpRequestFactoryBase
    {
        protected IRequestUrlGenerator RequestUrlGenerator { get; private set; }

        public ViewCleanupHttpRequestFactory(IDbClientConnection connection)
        {
            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        public ViewCleanupHttpRequestFactory(IServerClientConnection connection)
        {
            RequestUrlGenerator = new AppendingRequestUrlGenerator(connection.Address);
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