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

        public virtual async Task<BulkResponse> BulkAsync(BulkRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
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

        public virtual async Task<DocumentHeaderResponse> CopyAsync(CopyDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
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

        public virtual async Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> ExistsAsync(string id, string rev = null)
        {
            return ExistsAsync(new DocumentExistsRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> ExistsAsync(DocumentExistsRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentResponse> GetAsync(string id, string rev = null)
        {
            return GetAsync(new GetDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentResponse> GetAsync(GetDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessDocumentResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> PostAsync(string doc)
        {
            return PostAsync(new PostDocumentRequest(doc));
        }

        public virtual async Task<DocumentHeaderResponse> PostAsync(PostDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
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

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string id, string rev)
        {
            return DeleteAsync(new DeleteDocumentRequest(id, rev));
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteDocumentRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateHttpRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessDocumentHeaderResponse(res);
                }
            }
        }

        protected virtual HttpRequest CreateHttpRequest(BulkRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl(cmd));

            req.SetContent(cmd.ToJson());

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(CopyDocumentRequest cmd)
        {
            var req = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(cmd.SrcId, cmd.SrcRev));

            req.Headers.Add("Destination", cmd.NewId);

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(ReplaceDocumentRequest cmd)
        {
            var req = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(cmd.SrcId, cmd.SrcRev));

            req.Headers.Add("Destination", string.Concat(cmd.TrgId, "?rev=", cmd.TrgRev));

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(DocumentExistsRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Head, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(GetDocumentRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteDocumentRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(PutDocumentRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(cmd.Id, cmd.Rev));

            req.SetIfMatch(cmd.Rev);
            req.SetContent(cmd.Content);

            return req;
        }

        protected virtual HttpRequest CreateHttpRequest(PostDocumentRequest cmd)
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl());

            req.SetContent(cmd.Content);

            return req;
        }

        protected virtual string GenerateRequestUrl(BulkRequest cmd)
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