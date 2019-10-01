using EnsureThat;

namespace MyCouch.Requests
{
    public class PutAttachmentRequest : PutAttachmentRequestBase
    {
        public byte[] Content { get; private set; }

        public PutAttachmentRequest(string docId, string name, string contentType, byte[] content)
            : base(docId, name, contentType)
        {
            EnsureArg.HasItems(content, nameof(content));
            Content = content;
        }

        public PutAttachmentRequest(string docId, string docRev, string name, string contentType, byte[] content)
            : base(docId, docRev, name, contentType)
        {
            EnsureArg.HasItems(content, nameof(content));
            Content = content;
        }
    }
}