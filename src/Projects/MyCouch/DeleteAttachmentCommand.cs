using System;
using EnsureThat;

namespace MyCouch
{
    [Serializable]
    public class DeleteAttachmentCommand
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }

        public DeleteAttachmentCommand(string docId, string docRev, string name)
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