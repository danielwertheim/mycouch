using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class QueryResponseRow
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }
}