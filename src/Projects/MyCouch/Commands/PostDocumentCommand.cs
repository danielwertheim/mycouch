using System;
using EnsureThat;

namespace MyCouch.Commands
{
#if !WinRT
    [Serializable]
#endif
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