using System;
using MyCouch.Responses;

namespace MyCouch.Cloudant.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchQueryResponse<T> : QueryResponse<QueryResponseRow<T>> where T : class { }
}