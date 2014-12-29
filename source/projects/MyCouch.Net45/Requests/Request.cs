using System;

namespace MyCouch.Requests
{
#if !PCL
    [Serializable]
#endif
    public abstract class Request { }
}