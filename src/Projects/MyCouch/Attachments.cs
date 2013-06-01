using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class Attachments : IAttachments
    {
        protected readonly IClient Client;

        public Attachments(IClient client)
        {
            Ensure.That(client, "Client").IsNotNull();

            Client = client;
        }

        public virtual AttachmentResponse Get(string docId, string attachmentName)
        {
            return Get(new GetAttachmentCommand(docId, attachmentName));
        }

        public virtual AttachmentResponse Get(string docId, string docRev, string attachmentName)
        {
            return Get(new GetAttachmentCommand(docId, docRev, attachmentName));
        }

        public virtual Task<AttachmentResponse> GetAsync(string docId, string attachmentName)
        {
            return GetAsync(new GetAttachmentCommand(docId, attachmentName));
        }

        public virtual Task<AttachmentResponse> GetAsync(string docId, string docRev, string attachmentName)
        {
            return GetAsync(new GetAttachmentCommand(docId, docRev, attachmentName));
        }

        public virtual AttachmentResponse Get(GetAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return GetAsync(cmd).Result;
        }

        public virtual async Task<AttachmentResponse> GetAsync(GetAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessAttachmentResponseAsync(res);
        }

        public virtual DocumentHeaderResponse Put(PutAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return PutAsync(cmd).Result;
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessDocumentHeaderResponseAsync(res);
        }

        public virtual DocumentHeaderResponse Delete(DeleteAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            return DeleteAsync(cmd).Result;
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return await ProcessDocumentHeaderResponseAsync(res);
        }

        protected virtual HttpRequestMessage CreateRequest(GetAttachmentCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(cmd.DocId, cmd.DocRev, cmd.Name));

            req.SetIfMatch(cmd.DocRev);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(PutAttachmentCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(cmd.DocId, cmd.DocRev, cmd.Name));

            req.SetIfMatch(cmd.DocRev);
            req.SetContent(cmd.ContentType, cmd.Content);

            return req;
        }

        protected virtual HttpRequestMessage CreateRequest(DeleteAttachmentCommand cmd)
        {
            var req = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(cmd.DocId, cmd.DocRev, cmd.Name));

            req.SetIfMatch(cmd.DocRev);

            return req;
        }

        protected virtual string GenerateRequestUrl(string docId, string docRev, string attachmentName)
        {
            return string.Format("{0}/{1}/{2}{3}",
                Client.Connection.Address,
                docId,
                attachmentName,
                docRev == null ? string.Empty : string.Concat("?rev=", docRev));
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual async Task<AttachmentResponse> ProcessAttachmentResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateAttachmentResponse(await responseTask);
        }

        protected virtual async Task<DocumentHeaderResponse> ProcessDocumentHeaderResponseAsync(Task<HttpResponseMessage> responseTask)
        {
            return Client.ResponseFactory.CreateDocumentHeaderResponse(await responseTask);
        }
    }
}