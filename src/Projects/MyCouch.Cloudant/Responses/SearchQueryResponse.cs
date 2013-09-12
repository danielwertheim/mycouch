using System;
using MyCouch.Responses;

namespace MyCouch.Cloudant.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class SearchQueryResponse<T> : QueryResponse<SearchQueryResponse<T>.Row> where T : class
    {
#if !NETFX_CORE
        [Serializable]
#endif
        public class Row : QueryResponseRow
        {
            public T Value { get; set; }
            public T Doc { get; set; }
        }
    }
}