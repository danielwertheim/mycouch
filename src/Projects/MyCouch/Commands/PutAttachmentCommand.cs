using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutAttachmentCommand : ICommand
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public PutAttachmentCommand(string docId, string name, string contentType, byte[] content)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();
            Ensure.That(contentType, "contentType").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNull().HasItems();

            DocId = docId;
            Name = name;
            ContentType = contentType;
            Content = content;
        }

        public PutAttachmentCommand(string docId, string docRev, string name, string contentType, byte[] content) 
            : this(docId, name, contentType, content)
        {
            Ensure.That(docRev, "docRev").IsNotNullOrWhiteSpace();
            DocRev = docRev;
        }
    }
}