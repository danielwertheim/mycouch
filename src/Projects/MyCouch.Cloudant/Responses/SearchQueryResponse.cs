using System;
using MyCouch.Responses;

namespace MyCouch.Cloudant.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchQueryResponse<T> : QueryResponse<T> where T : class { }
}