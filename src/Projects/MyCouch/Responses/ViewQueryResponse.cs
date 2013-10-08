using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse : ViewQueryResponse<string> { }

#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryResponse<T> : QueryResponse<ViewQueryResponse<T>.Row>
    {
#if !NETFX_CORE
        [Serializable]
#endif
        public class Row : QueryResponseRow
        {
            public T Value { get; set; }
            public T IncludedDoc { get; set; }
        }
    }
}