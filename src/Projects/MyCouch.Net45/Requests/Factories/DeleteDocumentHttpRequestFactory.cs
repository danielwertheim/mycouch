using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public DeleteDocumentHttpRequestFactory(IConnection connection) : base(connection) {}

        public virtual HttpRequest Create(DeleteDocumentRequest request)
        {
            var httpRequest = CreateFor<DeleteDocumentRequest>(HttpMethod.Delete, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}