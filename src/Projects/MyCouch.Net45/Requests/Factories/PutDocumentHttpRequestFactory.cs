using System.Net.Http;
using EnsureThat;
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
            httpRequest.SetJsonContent(request.Content);

            return httpRequest;
        }

        protected override string GenerateRequestUrl(string id = null, string rev = null, bool batch = false)
        {
            Ensure.That(id, "id")
                .WithExtraMessageOf(() => "PUT requests must have an id part of the URL.")
                .IsNotNullOrWhiteSpace();

            return base.GenerateRequestUrl(id, rev, batch);
        }
    }
}