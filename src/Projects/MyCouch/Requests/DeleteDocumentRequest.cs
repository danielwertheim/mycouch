using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DeleteDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public DeleteDocumentRequest(string id, string rev)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();
            Ensure.That(rev, "rev").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}