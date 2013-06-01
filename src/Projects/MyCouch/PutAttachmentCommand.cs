using System;
using EnsureThat;

namespace MyCouch
{
    [Serializable]
    public class PutAttachmentCommand
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string AttachmentId { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public PutAttachmentCommand(string docId, string docRev, string attachmentId, string contentType, byte[] content)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(docRev, "docRev").IsNotNullOrWhiteSpace();
            Ensure.That(attachmentId, "attachmentId").IsNotNullOrWhiteSpace();
            Ensure.That(contentType, "contentType").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNull().HasItems();

            DocId = docId;
            DocRev = docRev;
            AttachmentId = attachmentId;
            ContentType = contentType;
            Content = content;
        }
    }
}