using System;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public abstract class Request { }
}