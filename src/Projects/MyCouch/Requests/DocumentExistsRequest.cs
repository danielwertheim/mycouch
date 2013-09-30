using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DocumentExistsRequest : IRequest
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public DocumentExistsRequest(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}