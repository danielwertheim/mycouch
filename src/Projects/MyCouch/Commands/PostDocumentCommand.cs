using System;
using EnsureThat;

namespace MyCouch.Commands
{
    [Serializable]
    public class PostDocumentCommand : IMyCouchCommand
    {
        public string Content { get; set; }

        public PostDocumentCommand(string content)
        {
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Content = content;
        }
    }
}