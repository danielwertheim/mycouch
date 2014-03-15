using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class HeadDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public HeadDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(HeadDocumentRequest request)
        {
            var httpRequest = CreateFor<HeadDocumentRequest>(HttpMethod.Head, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}