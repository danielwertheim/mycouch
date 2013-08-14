using System;

namespace MyCouch
{
#if !WinRT
    [Serializable]
#endif
    public class JsonViewQueryResponse : ViewQueryResponse<string> { }
}