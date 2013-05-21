using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class Documents : IDocuments
    {
        protected readonly IClient Client;

        public Documents(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual BulkResponse Bulk(BulkCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return BulkAsync(cmd).Result;
        }

        public virtual async Task<BulkResponse> BulkAsync(BulkCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessHttpBulkResponseAsync(res);
        }

        public virtual DocumentHeaderResponse Copy(string srcId, string newId)
        {
            return Copy(new CopyDocumentCommand(srcId, newId));
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string newId)
        {
            return CopyAsync(new CopyDocumentCommand(srcId, newId));
        }

        public virtual DocumentHeaderResponse Copy(string srcId, string srcRev, string newId)
        {
            return Copy(new CopyDocumentCommand(srcId, srcRev, newId));
        }

        public virtual Task<DocumentHeaderResponse> CopyAsync(string srcId, string srcRev, string newId)
        {
            return CopyAsync(new CopyDocumentCommand(srcId, srcRev, newId));
        }

        public virtual DocumentHeaderResponse Copy(CopyDocumentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return CopyAsync(cmd).Result;
        }

        public virtual async Task<DocumentHeaderResponse> CopyAsync(CopyDocumentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessHttpDocumentHeaderResponseAsync(res);
        }

        public virtual DocumentHeaderResponse Replace(string srcId, string trgId, string trgRev)
        {
            return Replace(new ReplaceDocumentCommand(srcId, trgId, trgRev));
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string trgId, string trgRev)
        {
            return ReplaceAsync(new ReplaceDocumentCommand(srcId, trgId, trgRev));
        }

        public virtual DocumentHeaderResponse Replace(string srcId, string srcRev, string trgId, string trgRev)
        {
            return Replace(new ReplaceDocumentCommand(srcId, srcRev, trgId, trgRev));
        }

        public virtual Task<DocumentHeaderResponse> ReplaceAsync(string srcId, string srcRev, string trgId, string trgRev)
        {
            return ReplaceAsync(new ReplaceDocumentCommand(srcId, srcRev, trgId, trgRev));
        }

        public virtual DocumentHeaderResponse Replace(ReplaceDocumentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return ReplaceAsync(cmd).Result;
        }

        public virtual async Task<DocumentHeaderResponse> ReplaceAsync(ReplaceDocumentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessHttpDocumentHeaderResponseAsync(res);
        }

        public virtual JsonDocumentResponse Get(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            return GetAsync(id, rev).Result;
        }

        public virtual async Task<JsonDocumentResponse> GetAsync(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Get, new JsonDocumentCommand { Id = id, Rev = rev });
            var res = SendAsync(req);

            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual JsonDocumentResponse Post(string doc)
        {
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            return PostAsync(doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PostAsync(string doc)
        {
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Post, new JsonDocumentCommand { Content = doc });
            var res = SendAsync(req);

            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual JsonDocumentResponse Put(string id, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            return PutAsync(id, doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PutAsync(string id, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new JsonDocumentCommand { Id = id, Content = doc });
            var res = SendAsync(req);

            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual JsonDocumentResponse Put(string id, string rev, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            return PutAsync(id, rev, doc).Result;
        }

        public virtual async Task<JsonDocumentResponse> PutAsync(string id, string rev, string doc)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(doc, "doc").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Put, new JsonDocumentCommand { Id = id, Rev = rev, Content = doc });
            var res = SendAsync(req);

            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        public virtual JsonDocumentResponse Delete(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            return DeleteAsync(id, rev).Result;
        }

        public virtual async Task<JsonDocumentResponse> DeleteAsync(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            var req = CreateRequest(HttpMethod.Delete, new JsonDocumentCommand { Id = id, Rev = rev });
            var res = SendAsync(req);

            return await ProcessHttpJsonDocumentResponseAsync(res);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual HttpRequestMessage CreateRequest(BulkCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Post, GenerateRequestUrl(cmd));

            req.SetContent(cmd.ToJson());

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(CopyDocumentCommand cmd)
        {
            var req = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(cmd));

            req.Headers.Add("Destination", cmd.NewId);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(ReplaceDocumentCommand cmd)
        {
            var req = new HttpRequest(new HttpMethod("COPY"), GenerateRequestUrl(cmd));

            req.Headers.Add("Destination", string.Concat(cmd.TrgId, "?rev=", cmd.TrgRev));

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, JsonDocumentCommand cmd)
        {
            var req = new HttpRequest(method, GenerateRequestUrl(cmd));

            if (!string.IsNullOrWhiteSpace(cmd.Rev))
                req.SetIfMatch(cmd.Rev);

            req.SetContent(cmd.Content);

            return req;
        }

        protected virtual string GenerateRequestUrl(BulkCommand cmd)
        {
            return string.Format("{0}/_bulk_docs", Client.Connection.Address);
        }

        protected virtual string GenerateRequestUrl(CopyDocumentCommand cmd)
        {
            return GenerateDocumentRequestUrl(cmd.SrcId, cmd.SrcRev);
        }

        protected virtual string GenerateRequestUrl(ReplaceDocumentCommand cmd)
        {
            return GenerateDocumentRequestUrl(cmd.SrcId, cmd.SrcRev);
        }

        protected virtual string GenerateRequestUrl(JsonDocumentCommand cmd)
        {
            return GenerateDocumentRequestUrl(cmd.Id, cmd.Rev);
        }

        protected virtual string GenerateDocumentRequestUrl(string id, string rev = null)
        {
            return string.Format("{0}/{1}{2}",
                Client.Connection.Address,
                id,
                rev == null ? string.Empty : string.Concat("?rev=", rev));
        }

        protected virtual async Task<BulkResponse> ProcessHttpBulkResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateBulkResponse(await responseTask);
        }

        protected virtual async Task<DocumentHeaderResponse> ProcessHttpDocumentHeaderResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateDocumentHeaderResponse(await responseTask);
        }

        protected virtual async Task<JsonDocumentResponse> ProcessHttpJsonDocumentResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateJsonDocumentResponse(await responseTask);
        }

        [Serializable]
        protected internal class JsonDocumentCommand
        {
            public string Id { get; set; }
            public string Rev { get; set; }
            public string Content { get; set; }
        }
    }
}