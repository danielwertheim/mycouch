using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetAttachmentHttpRequestFactory : AttachmentHttpRequestFactoryBase
    {
        public GetAttachmentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(GetAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<GetAttachmentRequest>(HttpMethod.Get, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);

            return httpRequest;
        }
    }
}
