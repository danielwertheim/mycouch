using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PutDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public string Content { get; set; }
        public bool Batch { get; set; }

        public PutDocumentRequest(string id, string rev, string content)
            : this(id, content)
        {
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            Initialize();
            Rev = rev;
        }

        public PutDocumentRequest(string id, string content)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(content, "content").IsNotNullOrWhiteSpace();

            Initialize();
            Id = id;
            Content = content;
        }

        private void Initialize()
        {
            Batch = false;
        }
    }
}