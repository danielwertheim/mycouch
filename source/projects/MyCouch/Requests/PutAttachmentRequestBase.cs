using EnsureThat;

namespace MyCouch.Requests
{
    public abstract class PutAttachmentRequestBase : Request
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }

        internal PutAttachmentRequestBase(string docId, string name, string contentType)
        {
            EnsureArg.IsNotNullOrWhiteSpace(docId, nameof(docId));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            EnsureArg.IsNotNullOrWhiteSpace(contentType, nameof(contentType));

            DocId = docId;
            Name = name;
            ContentType = contentType;
        }

        internal PutAttachmentRequestBase(string docId, string docRev, string name, string contentType)
            : this(docId, name, contentType)
        {
            EnsureArg.IsNotNullOrWhiteSpace(docRev, nameof(docRev));
            DocRev = docRev;
        }
    }
}