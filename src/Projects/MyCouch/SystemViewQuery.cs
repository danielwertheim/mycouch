using System;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SystemViewQuery : ViewQuery
    {
        public SystemViewQuery(string viewName) : base("SYSTEMVIEW", viewName) { }
    }
}