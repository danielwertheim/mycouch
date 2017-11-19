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
    public class Attachments : ApiContextBase<IDbConnection>, IAttachments
    {
        protected AttachmentResponseFactory AttachmentResponseFactory { get; set; }
        protected DocumentHeaderResponseFactory DocumentHeaderResponseFactory { get; set; }
        protected GetAttachmentHttpRequestFactory GetAttachmentHttpRequestFactory { get; set; }
        protected PutAttachmentHttpRequestFactory PutAttachmentHttpRequestFactory { get; set; }
        protected DeleteAttachmentHttpRequestFactory DeleteAttachmentHttpRequestFactory { get; set; }

        public Attachments(IDbConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            AttachmentResponseFactory = new AttachmentResponseFactory(serializer);
            DocumentHeaderResponseFactory = new DocumentHeaderResponseFactory(serializer);
            GetAttachmentHttpRequestFactory = new GetAttachmentHttpRequestFactory();
            PutAttachmentHttpRequestFactory = new PutAttachmentHttpRequestFactory();
            DeleteAttachmentHttpRequestFactory = new DeleteAttachmentHttpRequestFactory();
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
            var httpRequest = GetAttachmentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await AttachmentResponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual async Task<DocumentHeaderResponse> PutAsync(PutAttachmentRequest request)
        {
            var httpRequest = PutAttachmentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderResponseFactory.CreateAsync(res).ForAwait();
            }
        }

        public virtual Task<DocumentHeaderResponse> DeleteAsync(string docId, string docRev, string attachmentName)
        {
            return DeleteAsync(new DeleteAttachmentRequest(docId, docRev, attachmentName));
        }

        public virtual async Task<DocumentHeaderResponse> DeleteAsync(DeleteAttachmentRequest request)
        {
            var httpRequest = DeleteAttachmentHttpRequestFactory.Create(request);

            using (var res = await SendAsync(httpRequest).ForAwait())
            {
                return await DocumentHeaderResponseFactory.CreateAsync(res).ForAwait();
            }
        }
    }
}