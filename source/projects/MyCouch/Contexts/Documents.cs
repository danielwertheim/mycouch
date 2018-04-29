using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Documents : ApiContextBase<IDbConnection>, IDocuments
    {
        protected BulkHttpRequestFactory BulkHttpRequestFactory { get; set; }
        protected CopyDocumentHttpRequestFactory CopyDocumentHttpRequestFactory { get; set; }
        protected ReplaceDocumentHttpRequestFactory ReplaceDocumentHttpRequestFactory { get; set; }
        protected HeadDocumentHttpRequestFactory HeadDocumentHttpRequestFactory { get; set; }
        protected GetDocumentHttpRequestFactory GetDocumentHttpRequestFactory { get; set; }
        protected PostDocumentHttpRequestFactory PostDocumentHttpRequestFactory { get; set; }
        protected PutDocumentHttpRequestFactory PutDocumentHttpRequestFactory { get; set; }
        protected DeleteDocumentHttpRequestFactory DeleteDocumentHttpRequestFactory { get; set; }
        protected QueryShowHttpRequestFactory QueryShowHttpRequestFactory { get; set; }
        protected DocumentResponseFactory DocumentReponseFactory { get; set; }
        protected DocumentHeaderResponseFactory DocumentHeaderReponseFactory { get; set; }
        protected BulkResponseFactory BulkReponseFactory { get; set; }
        protected RawResponseFactory RawResponseFactory { get; set; }

        public ISerializer Serializer { get; private set; }

        public Documents(IDbConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
            BulkHttpRequestFactory = new BulkHttpRequestFactory();
            CopyDocumentHttpRequestFactory = new CopyDocumentHttpRequestFactory();
            ReplaceDocumentHttpRequestFactory = new ReplaceDocumentHttpRequestFactory();
            HeadDocumentHttpRequestFactory = new HeadDocumentHttpRequestFactory();
            GetDocumentHttpRequestFactory = new GetDocumentHttpRequestFactory();
            PostDocumentHttpRequestFactory = new PostDocumentHttpRequestFactory();
            PutDocumentHttpRequestFactory = new PutDocumentHttpRequestFactory();
            DeleteDocumentHttpRequestFactory = new DeleteDocumentHttpRequestFactory();
            QueryShowHttpRequestFactory = new QueryShowHttpRequestFactory(Serializer);

            DocumentReponseFactory = new DocumentResponseFactory(Serializer);
            DocumentHeaderReponseFactory = new DocumentHeaderResponseFactory(Serializer);
            BulkReponseFactory = new BulkResponseFactory(Serializer);
            RawResponseFactory = new RawResponseFactory(Serializer);
        }

        public virtual async Task<BulkResponse> BulkAsync(BulkRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = BulkHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await BulkReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId, CancellationToken cancellationToken = default)
        {
            return CopyAsync(new CopyDocumentRequest(srcId, newId), cancellationToken);
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId, CancellationToken cancellationToken = default)
        {
            return CopyAsync(new CopyDocumentRequest(srcId, srcRev, newId), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> CopyAsync(CopyDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = CopyDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string trgId, string trgRev, CancellationToken cancellationToken = default)
        {
            return ReplaceAsync(new ReplaceDocumentRequest(srcId, trgId, trgRev), cancellationToken);
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev, CancellationToken cancellationToken = default)
        {
            return ReplaceAsync(new ReplaceDocumentRequest(srcId, srcRev, trgId, trgRev), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = ReplaceDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> HeadAsync(string id, string rev = null, CancellationToken cancellationToken = default)
        {
            return HeadAsync(new HeadDocumentRequest(id, rev), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> HeadAsync(HeadDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = HeadDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentResponse> GetAsync(string id, string rev = null, CancellationToken cancellationToken = default)
        {
            return GetAsync(new GetDocumentRequest(id, rev), cancellationToken);
        }

        public virtual async Task<DocumentResponse> GetAsync(GetDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = GetDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> PostAsync(string doc, CancellationToken cancellationToken = default)
        {
            return PostAsync(new PostDocumentRequest(doc), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = PostDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string doc, CancellationToken cancellationToken = default)
        {
            return PutAsync(PutDocumentRequest.ForCreate(id, doc), cancellationToken);
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc, CancellationToken cancellationToken = default)
        {
            return PutAsync(PutDocumentRequest.ForUpdate(id, rev, doc), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = PutDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string id, string rev, CancellationToken cancellationToken = default)
        {
            return DeleteAsync(new DeleteDocumentRequest(id, rev), cancellationToken);
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = DeleteDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<RawResponse> ShowAsync(QueryShowRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = QueryShowHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest, cancellationToken).ForAwait())
            {
                return await RawResponseFactory.CreateAsync(res).ForAwait();
            }
        }
    }
}