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
    public class Attachments : ApiContextBase, IAttachments
    {
        protected AttachmentResponseFactory AttachmentResponseFactory { get; set; }
        protected DocumentHeaderResponseFactory DocumentHeaderResponseFactory { get; set; }

        public Attachments(IConnection connection, SerializationConfiguration serializationConfiguration) : base(connection)
        {
            Ensure.That(serializationConfiguration, "serializationConfiguration").IsNotNull();

            AttachmentResponseFactory = new AttachmentResponseFactory(serializationConfiguration);
            DocumentHeaderResponseFactory = new DocumentHeaderResponseFactory(serializationConfiguration);
        }

        public virtual Task<AttachmentResponse> GetAsync(string docId, string attachmentName)
        {
            return GetAsync(new GetAttachmentRequest(docId, attachmentName));
        }

        public virtual Task<AttachmentResponse> GetAsync(string docId, string docRev, string attachmentName)
        {
            return GetAsync(new GetAttachmentRequest(docId, docRev, attachmentName));
        }

        public virtual async Task<AttachmentResponse> GetAsync(GetAttachmentRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            using (var httpRequest = CreateHttpRequest(request))
            {
                using (var res = await SendAsync(httpRequest).ForAwait())
                {
                    return ProcessAttachmentResponse(res);
                }
            }
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutAttachmentRequest request)
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

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string docId, string docRev, string attachmentName)
        {
            return DeleteAsync(new DeleteAttachmentRequest(docId, docRev, attachmentName));
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentRequest request)
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

        protected virtual HttpRequest CreateHttpRequest(GetAttachmentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Get, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(PutAttachmentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Put, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);
            httpRequest.SetContent(request.ContentType, request.Content);

            return httpRequest;
        }

        protected virtual HttpRequest CreateHttpRequest(DeleteAttachmentRequest request)
        {
            var httpRequest = new HttpRequest(HttpMethod.Delete, GenerateRequestUrl(request.DocId, request.DocRev, request.Name));

            httpRequest.SetIfMatch(request.DocRev);

            return httpRequest;
        }

        protected virtual string GenerateRequestUrl(string docId, string docRev, string attachmentName)
        {
            return string.Format("{0}/{1}/{2}{3}",
                Connection.Address,
                docId,
                attachmentName,
                docRev == null ? string.Empty : string.Concat("?rev=", docRev));
        }

        protected virtual AttachmentResponse ProcessAttachmentResponse(HttpResponseMessage response)
        {
            return AttachmentResponseFactory.Create(response);
        }

        protected virtual DocumentHeaderResponse ProcessDocumentHeaderResponse(HttpResponseMessage response)
        {
            return DocumentHeaderResponseFactory.Create(response);
        }
    }
}