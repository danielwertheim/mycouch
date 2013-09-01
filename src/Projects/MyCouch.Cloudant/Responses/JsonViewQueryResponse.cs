using System;

namespace MyCouch.Cloudant.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class JsonIndexQueryResponse : IndexQueryResponse<string> { }
}