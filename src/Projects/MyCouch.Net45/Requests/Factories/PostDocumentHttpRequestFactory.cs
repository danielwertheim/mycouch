using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PostDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public PostDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(PostDocumentRequest request)
        {
            var httpRequest = CreateFor<PostDocumentRequest>(HttpMethod.Post, GenerateRequestUrl(batch: request.Batch));

            httpRequest.SetContent(request.Content);

            return httpRequest;
        }
    }
}