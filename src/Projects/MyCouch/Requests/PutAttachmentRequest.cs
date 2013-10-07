using System;
using EnsureThat;
using System.Net.Http;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutAttachmentRequest : IRequest
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public PutAttachmentRequest(string docId, string name, string contentType, byte[] content)
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

        public PutAttachmentRequest(string docId, string docRev, string name, string contentType, byte[] content) 
            : this(docId, name, contentType, content)
        {
            Ensure.That(docRev, "docRev").IsNotNullOrWhiteSpace();
            DocRev = docRev;
        }
    }
}