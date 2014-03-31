using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PostDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public PostDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(PostDocumentRequest request)
        {
            var batchParam = request.Batch ? new UrlParam("batch", "ok") : null;
            var httpRequest = CreateFor<PostDocumentRequest>(
                HttpMethod.Post,
                GenerateRequestUrl(parameters: batchParam));

            httpRequest.SetJsonContent(request.Content);

            return httpRequest;
        }
    }
}