using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class CopyDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase, IHttpRequestFactory<CopyDocumentRequest>
    {
        public CopyDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(CopyDocumentRequest request)
        {
            var httpRequest = CreateFor<CopyDocumentRequest>(new HttpMethod("COPY"), GenerateRequestUrl(request.SrcId, request.SrcRev));

            httpRequest.Headers.Add("Destination", request.NewId);

            return httpRequest;
        }
    }
}