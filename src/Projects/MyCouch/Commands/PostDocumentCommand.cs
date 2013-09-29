using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PostDocumentCommand : ICommand
    {
        public string Content { get; set; }

        public PostDocumentCommand(string content)
        {
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Content = content;
        }
    }
}