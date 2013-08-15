using System;

namespace MyCouch
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class JsonViewQueryResponse : ViewQueryResponse<string> { }
}