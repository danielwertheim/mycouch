using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class ReplaceDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase, IHttpRequestFactory<ReplaceDocumentRequest>
    {
        public ReplaceDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(ReplaceDocumentRequest request)
        {
            var httpRequest = CreateFor<ReplaceDocumentRequest>(new HttpMethod("COPY"), GenerateRequestUrl(request.SrcId, request.SrcRev));

            httpRequest.Headers.Add("Destination", string.Concat(request.TrgId, "?rev=", request.TrgRev));

            return httpRequest;
        }
    }
}