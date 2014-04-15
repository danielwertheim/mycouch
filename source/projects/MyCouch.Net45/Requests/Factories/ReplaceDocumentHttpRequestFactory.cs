using System.Net.Http;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ReplaceDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public ReplaceDocumentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(ReplaceDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<ReplaceDocumentRequest>(new HttpMethod("COPY"), GenerateRequestUrl(request.SrcId, request.SrcRev));

            httpRequest.Headers.Add("Destination", string.Concat(request.TrgId, "?rev=", request.TrgRev));

            return httpRequest;
        }
    }
}