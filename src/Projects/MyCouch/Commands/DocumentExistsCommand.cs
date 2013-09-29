using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentExistsCommand : ICommand
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