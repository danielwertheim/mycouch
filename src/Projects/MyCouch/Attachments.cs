using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Commands;
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

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessAttachmentResponse(res);
        }

        public virtual async Task<AttachmentResponse> GetAsync(GetAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessAttachmentResponse(await res);
        }

        public virtual DocumentHeaderResponse Put(PutAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessDocumentHeaderResponse(res);
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessDocumentHeaderResponse(await res);
        }

        public virtual DocumentHeaderResponse Delete(string docId, string docRev, string attachmentName)
        {
            return Delete(new DeleteAttachmentCommand(docId, docRev, attachmentName));
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string docId, string docRev, string attachmentName)
        {
            return DeleteAsync(new DeleteAttachmentCommand(docId, docRev, attachmentName));
        }

        public virtual DocumentHeaderResponse Delete(DeleteAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = Send(req);

            return ProcessDocumentHeaderResponse(res);
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentCommand cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            var req = CreateRequest(cmd);
            var res = SendAsync(req);

            return ProcessDocumentHeaderResponse(await res);
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

        protected virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return Client.Connection.Send(request);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.Connection.SendAsync(request);
        }

        protected virtual AttachmentResponse ProcessAttachmentResponse(HttpResponseMessage response)
        {
            return Client.ResponseFactory.CreateAttachmentResponse(response);
        }

        protected virtual DocumentHeaderResponse ProcessDocumentHeaderResponse(HttpResponseMessage response)
        {
            return Client.ResponseFactory.CreateDocumentHeaderResponse(response);
        }
    }
}