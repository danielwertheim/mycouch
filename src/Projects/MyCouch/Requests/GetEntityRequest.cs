using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class GetEntityRequest : IRequest
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }

        public GetEntityRequest(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}