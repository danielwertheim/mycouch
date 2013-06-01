using System;
using EnsureThat;

namespace MyCouch
{
    [Serializable]
    public class DeleteAttachmentCommand
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string AttachmentId { get; private set; }

        public DeleteAttachmentCommand(string docId, string docRev, string attachmentId)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(docRev, "docRev").IsNotNullOrWhiteSpace();
            Ensure.That(attachmentId, "attachmentId").IsNotNullOrWhiteSpace();

            DocId = docId;
            DocRev = docRev;
            AttachmentId = attachmentId;
        }
    }
}