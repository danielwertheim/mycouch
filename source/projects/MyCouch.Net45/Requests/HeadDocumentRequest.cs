using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class HeadDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public HeadDocumentRequest(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}