using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public GetDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(GetDocumentRequest request)
        {
            var httpRequest = CreateFor<GetDocumentRequest>(HttpMethod.Get, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}