using System.Net.Http;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class PutDocumentHttpRequestFactory : DocumentHttpRequestFactoryBase
    {
        public PutDocumentHttpRequestFactory(IConnection connection) : base(connection) { }

        public virtual HttpRequest Create(PutDocumentRequest request)
        {
            var httpRequest = CreateFor<PutDocumentRequest>(HttpMethod.Put, GenerateRequestUrl(request.Id, request.Rev, request.Batch));

            httpRequest.SetIfMatch(request.Rev);
            httpRequest.SetContent(request.Content);

            return httpRequest;
        }
    }
}