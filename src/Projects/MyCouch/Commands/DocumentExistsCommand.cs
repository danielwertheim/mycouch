using System;
using EnsureThat;

namespace MyCouch.Commands
{
    [Serializable]
    public class DocumentExistsCommand
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public DocumentExistsCommand(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}