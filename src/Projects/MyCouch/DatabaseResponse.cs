using System;

namespace MyCouch
{
#if !WinRT
    [Serializable]
#endif
    public class DatabaseResponse : Response { }
}