using System;

namespace MyCouch
{
    [Serializable]
    public class SystemViewQuery : ViewQuery, ISystemViewQuery
    {
        public SystemViewQuery(string viewName) : base("SYSTEMVIEW", viewName) { }
    }
}