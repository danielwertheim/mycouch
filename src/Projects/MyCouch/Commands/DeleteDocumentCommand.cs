using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DeleteDocumentCommand : ICommand
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