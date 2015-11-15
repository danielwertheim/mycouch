using System.Net.Http;
using System.Threading.Tasks;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Net;
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
            Ensure.That(serializer, "serializer").IsNotNull();

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

        public virtual async Task<BulkResponse> BulkAsync(BulkRequest request)
        {
            var httpRequest = BulkHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await BulkReponseFactory.CreateAsync(res).ForAwait();
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
            var httpRequest = CopyDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
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
            var httpRequest = ReplaceDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> HeadAsync(string id, string rev = null)
        {
            return HeadAsync(new HeadDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> HeadAsync(HeadDocumentRequest request)
        {
            var httpRequest = HeadDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentResponse> GetAsync(string id, string rev = null)
        {
            return GetAsync(new GetDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentResponse> GetAsync(GetDocumentRequest request)
        {
            var httpRequest = GetDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> PostAsync(string doc)
        {
            return PostAsync(new PostDocumentRequest(doc));
        }

        public virtual async Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest request)
        {
            var httpRequest = PostDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string doc)
        {
            return PutAsync(PutDocumentRequest.ForCreate(id, doc));
        }

        public virtual Task<DocumentHeaderResponse> PutAsync(string id, string rev, string doc)
        {
            return PutAsync(PutDocumentRequest.ForUpdate(id, rev, doc));
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest request)
        {
            var httpRequest = PutDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string id, string rev)
        {
            return DeleteAsync(new DeleteDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest request)
        {
            var httpRequest = DeleteDocumentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderReponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<RawResponse> ShowAsync(QueryShowRequest request)
        {
            var httpRequest = QueryShowHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await RawResponseFactory.CreateAsync(res).ForAwait();
            }
        }
    }
}