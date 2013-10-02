using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Documents : ApiContextBase, IDocuments
    {
        protected DocumentResponseFactory DocumentReponseFactory { get; set; }
        protected DocumentHeaderResponseFactory DocumentHeaderReponseFactory { get; set; }
        protected BulkResponseFactory BulkReponseFactory { get; set; }

        public Documents(IConnection connection, SerializationConfiguration serializationConfiguration) : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            DocumentReponseFactory = new DocumentResponseFactory(serializationConfiguration);
            DocumentHeaderReponseFactory = new DocumentHeaderResponseFactory(serializationConfiguration);
            BulkReponseFactory = new BulkResponseFactory(serializationConfiguration);
        }

        public virtual async Task<BulkResponse> BulkAsync(BulkRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessBulkResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId)
        {
            return CopyAsync(new CopyDocumentRequest(srcId, newId));
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId)
        {
            return CopyAsync(new CopyDocumentRequest(srcId, srcRev, newId));
        }

        public virtual async Task<DocumentHeaderResponse> CopyAsync(CopyDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            return ReplaceAsync(new ReplaceDocumentRequest(srcId, trgId, trgRev));
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            return ReplaceAsync(new ReplaceDocumentRequest(srcId, srcRev, trgId, trgRev));
        }

        public virtual async Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> ExistsAsync(string id, string rev = null)
        {
            return ExistsAsync(new DocumentExistsRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> ExistsAsync(DocumentExistsRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentResponse> GetAsync(string id, string rev = null)
        {
            return GetAsync(new GetDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentResponse> GetAsync(GetDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> PostAsync(string doc)
        {
            return PostAsync(new PostDocumentRequest(doc));
        }

        public virtual async Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string doc)
        {
            return PutAsync(new PutDocumentRequest(id, doc));
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc)
        {
            return PutAsync(new PutDocumentRequest(id, rev, doc));
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string id, string rev)
        {
            return DeleteAsync(new DeleteDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(BulkRequest request)
        {
            var createHttpRequest = new HttpRequest(HttpMethod.Post, GenerateRequestUrl(request));

            createHttpRequest.SetContent(request.ToJson());

            return createHttpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(CopyDocumentRequest request)
        {
            var httpRequest = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(request.SrcId, request.SrcRev));

            httpRequest.Headers.Add("Destination", request.NewId);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(ReplaceDocumentRequest request)
        {
            var httpRequest = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(request.SrcId, request.SrcRev));

            httpRequest.Headers.Add("Destination", string.Concat(request.TrgId, "?rev=", request.TrgRev));

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(DocumentExistsRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Head, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(GetDocumentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteDocumentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(PutDocumentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(request.Id, request.Rev));

            httpRequest.SetIfMatch(request.Rev);
            httpRequest.SetContent(request.Content);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(PostDocumentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Post, GenerateRequestUrl());

            httpRequest.SetContent(request.Content);

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(BulkRequest request)
        {
            return string.Format("{0}/_bulk_docs", Connection.Address);
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual BulkResponse ProcessBulkResponse(HttpResponseMessage response)
        {
            return BulkReponseFactory.Create(response);
        }

        protected virtual DocumentHeaderResponse ProcessDocumentHeaderResponse(HttpResponseMessage response)
        {
            return DocumentHeaderReponseFactory.Create(response);
        }

        protected virtual DocumentResponse ProcessDocumentResponse(HttpResponseMessage response)
        {
            return DocumentReponseFactory.Create(response);
        }
    }
}