using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PostDocumentRequest : IRequest
    {
        public string Content { get; set; }

        public PostDocumentRequest(string content)
        {
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Content = content;
        }
    }
}