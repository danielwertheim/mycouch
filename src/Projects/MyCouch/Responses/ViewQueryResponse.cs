using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse<T> : QueryResponse<T> where T : class { }
}