using System.Net.Http;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class HeadDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public HeadDocumentHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public virtual HttpRequest Create(HeadDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = CreateFor<HeadDocumentRequest>(HttpMethod.Head, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}