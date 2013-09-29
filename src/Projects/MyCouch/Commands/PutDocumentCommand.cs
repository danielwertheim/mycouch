using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutDocumentCommand : ICommand
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public string Content { get; set; }

        public PutDocumentCommand(string id, string rev, string content)
            : this(id, content)
        {
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            Rev = rev;
        }

        public PutDocumentCommand(string id, string content)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Id = id;
            Content = content;
        }
    }
}