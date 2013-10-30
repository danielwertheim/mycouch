using System;

namespace MyCouch.Requests
{
    /// <summary>
    /// Used to query builtin system views, e.g. the all_keys view.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class QuerySystemViewRequest : QueryViewRequest
    {
        public QuerySystemViewRequest(string viewName) : base("SYSTEMVIEW", viewName) { }
    }
}