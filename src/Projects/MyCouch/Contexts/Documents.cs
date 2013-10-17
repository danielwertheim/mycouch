using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Documents : ApiContextBase, IDocuments
    {
        protected IHttpRequestFactory<BulkRequest> BulkHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<CopyDocumentRequest> CopyDocumentHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<ReplaceDocumentRequest> ReplaceDocumentHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<DocumentExistsRequest> DocumentExistsHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<GetDocumentRequest> GetDocumentHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<PostDocumentRequest> PostDocumentHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<PutDocumentRequest> PutDocumentHttpRequestFactory { get; set; }
        protected IHttpRequestFactory<DeleteDocumentRequest> DeleteDocumentHttpRequestFactory { get; set; }
        protected DocumentResponseFactory DocumentReponseFactory { get; set; }
        protected DocumentHeaderResponseFactory DocumentHeaderReponseFactory { get; set; }
        protected BulkResponseFactory BulkReponseFactory { get; set; }

        public Documents(IConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            BulkHttpRequestFactory = new BulkRequestHttpRequestFactory(Connection);
            CopyDocumentHttpRequestFactory = new CopyDocumentHttpRequestFactory(Connection);
            ReplaceDocumentHttpRequestFactory = new ReplaceDocumentHttpRequestFactory(Connection);
            DocumentExistsHttpRequestFactory = new DocumentExistsHttpRequestFactory(Connection);
            GetDocumentHttpRequestFactory = new GetDocumentHttpRequestFactory(Connection);
            PostDocumentHttpRequestFactory = new PostDocumentHttpRequestFactory(Connection);
            PutDocumentHttpRequestFactory = new PutDocumentHttpRequestFactory(Connection);
            DeleteDocumentHttpRequestFactory = new DeleteDocumentHttpRequestFactory(Connection);

            DocumentReponseFactory = new DocumentResponseFactory(serializer);
            DocumentHeaderReponseFactory = new DocumentHeaderResponseFactory(serializer);
            BulkReponseFactory = new BulkResponseFactory(serializer);
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
            return BulkHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(CopyDocumentRequest request)
        {
            return CopyDocumentHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(ReplaceDocumentRequest request)
        {
            return ReplaceDocumentHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(DocumentExistsRequest request)
        {
            return DocumentExistsHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(GetDocumentRequest request)
        {
            return GetDocumentHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteDocumentRequest request)
        {
            return DeleteDocumentHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(PutDocumentRequest request)
        {
            return PutDocumentHttpRequestFactory.Create(request);
        }

        protected virtual HttpRequest CreateHttpRequest(PostDocumentRequest request)
        {
            return PostDocumentHttpRequestFactory.Create(request);
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