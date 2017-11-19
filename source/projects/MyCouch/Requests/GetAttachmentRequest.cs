using EnsureThat;

namespace MyCouch.Requests
{
    public class GetAttachmentRequest : Request
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }

        public GetAttachmentRequest(string docId, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(docId, nameof(docId));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            DocId = docId;
            Name = name;
        }

        public GetAttachmentRequest(string docId, string docRev, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(docId, nameof(docId));
            EnsureArg.IsNotNullOrWhiteSpace(docRev, nameof(docRev));
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

            DocId = docId;
            DocRev = docRev;
            Name = name;
        }
    }
}