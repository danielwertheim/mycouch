using System.IO;
using EnsureThat;

namespace MyCouch.Requests
{
    public class PutAttachmentStreamRequest : PutAttachmentRequestBase
    {
        public Stream Content { get; private set; }

        public PutAttachmentStreamRequest(string docId, string name, string contentType, Stream content)
            : base(docId, name, contentType)
        {
            EnsureArg.IsNotNull(content, nameof(content));
            Content = content;
        }

        public PutAttachmentStreamRequest(string docId, string docRev, string name, string contentType, Stream content)
            : base(docId, docRev, name, contentType)
        {
            EnsureArg.IsNotNull(content, nameof(content));
            Content = content;
        }
    }
}