using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class GetAttachmentCommand : ICommand
    {
        public string DocId { get; private set; }
        public string DocRev { get; private set; }
        public string Name { get; private set; }

        public GetAttachmentCommand(string docId, string name)
        {
            Ensure.That(docId, "docId").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DocId = docId;
            Name = name;
        }

        public GetAttachmentCommand(string docId, string docRev, string name)
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