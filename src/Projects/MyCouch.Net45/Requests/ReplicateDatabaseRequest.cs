using System;
using EnsureThat;

namespace MyCouch.Requests
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ReplicateDatabaseRequest
    {
        public string Source { get; set; }
        public string Target { get; set; }

        public ReplicateDatabaseRequest(string source, string target)
        {
            Ensure.That(source, "source").IsNotNullOrWhiteSpace();
            Ensure.That(target, "target").IsNotNullOrWhiteSpace();

            Source = source;
            Target = target;
        }
    }
}