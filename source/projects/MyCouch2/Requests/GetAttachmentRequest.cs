using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if net45
    [Serializable]
#endif
    public class GetAttachmentRequest : Request
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }

        public GetAttachmentRequest(string docId, string name)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DocId = docId;
            Name = name;
        }

        public GetAttachmentRequest(string docId, string docRev, string name)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(docRev, "docRev").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DocId = docId;
            DocRev = docRev;
            Name = name;
        }
    }
}