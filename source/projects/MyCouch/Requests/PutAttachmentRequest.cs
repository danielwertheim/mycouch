using EnsureThat;

namespace MyCouch.Requests
{
    public class PutAttachmentRequest : Request
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public PutAttachmentRequest(string docId, string name, string contentType, byte[] content)
        {
            EnsureArg.IsNotNullOrWhiteSpace(docId, nameof(docId));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            EnsureArg.IsNotNullOrWhiteSpace(contentType, nameof(contentType));
            EnsureArg.HasItems(content, nameof(content));

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