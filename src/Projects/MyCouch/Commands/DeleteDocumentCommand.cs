using System;
using EnsureThat;

namespace MyCouch.Commands
{
    [Serializable]
    public class DeleteDocumentCommand
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public DeleteDocumentCommand(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}