using System;
using System.Collections.Generic;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public class ReplicateDatabaseRequest : Request
    {
        public string Source { get; private set; }
        public string Target { get; private set; }
        public string[] DocIds { get; set; }
        public bool? CreateTarget { get; set; }
        public bool? Continuous { get; set; }
        public bool? Cancel { get; set; }
        public string Proxy { get; set; }
        public string Filter { get; set; }
        public IDictionary<string, object> QueryParams { get; set; }

        public ReplicateDatabaseRequest(string source, string target)
        {
            Ensure.That(source, "source").IsNotNullOrWhiteSpace();
            Ensure.That(target, "target").IsNotNullOrWhiteSpace();

            Source = source;
            Target = target;
            QueryParams = new Dictionary<string, object>();
        }

        public virtual bool HasDocIds()
        {
            return DocIds != null && DocIds.Length > 0;
        }

        public virtual bool HasQueryParams()
        {
            return QueryParams != null && QueryParams.Count > 0;
        }
    }
}