using System;

namespace MyCouch.Querying
{
    [Serializable]
    public class SystemViewQuery : ViewQuery, ISystemViewQuery
    {
        public SystemViewQuery(string viewName) : base("SYSTEMVIEW", viewName) //TODO: Ugly
        {
        }
    }
}