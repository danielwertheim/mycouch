using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteAttachmentHttpRequestFactory : AttachmentHttpRequestFactoryBase
    {
        public DeleteAttachmentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(DeleteAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<DeleteAttachmentRequest>(HttpMethod.Delete, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);

            return httpRequest;
        }
    }
}