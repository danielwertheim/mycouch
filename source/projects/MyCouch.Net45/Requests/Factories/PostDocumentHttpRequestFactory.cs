using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PostDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public PostDocumentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(PostDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();
            Ensure.That(request.Content, "request.Content").IsNotNullOrWhiteSpace();

            var batchParam = request.Batch ? new UrlParam("batch", "ok") : null;
            var httpRequest = CreateFor<PostDocumentRequest>(
                HttpMethod.Post,
                GenerateRequestUrl(parameters: batchParam));

            httpRequest.SetJsonContent(request.Content);

            return httpRequest;
        }
    }
}