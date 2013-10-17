using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class DocumentExistsHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public DocumentExistsHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(DocumentExistsRequest request)
        {
            var httpRequest = CreateFor<DocumentExistsRequest>(HttpMethod.Head, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }
    }
}