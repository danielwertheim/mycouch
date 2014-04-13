using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PutAttachmentHttpRequestFactory : AttachmentHttpRequestFactoryBase
    {
        public PutAttachmentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(PutAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<PutAttachmentRequest>(HttpMethod.Put, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);
            httpRequest.SetContent(request.Content, request.ContentType);

            return httpRequest;
        }
    }
}