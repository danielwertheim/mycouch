using System;

namespace MyCouch
{
#if !WinRT
    [Serializable]
#endif
    public class SystemViewQuery : ViewQuery, ISystemViewQuery
    {
        public SystemViewQuery(string viewName) : base("SYSTEMVIEW", viewName) { }
    }
}