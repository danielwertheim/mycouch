using System;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class ContinuousChangesResponse : Response { }
}