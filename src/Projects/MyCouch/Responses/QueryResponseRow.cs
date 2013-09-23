using System;
#if NETFX_CORE
using System.Reflection;
#endif

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class QueryResponseRow
    {
#if NETFX_CORE
        public static readonly TypeInfo TypeInfo;

        static QueryResponseRow()
        {
            TypeInfo = typeof (QueryResponseRow).GetTypeInfo();
        }
#endif
        public string Id { get; set; }
        public object Key { get; set; }
    }
}