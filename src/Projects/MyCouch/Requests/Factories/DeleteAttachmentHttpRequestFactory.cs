using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DeleteAttachmentHttpRequestFactory :
        AttachmentHttpRequestFactoryBase,
        IHttpRequestFactory<DeleteAttachmentRequest>
    {
        public DeleteAttachmentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(DeleteAttachmentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);

            return httpRequest;
        }
    }
}