using System;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class ContinuousChangesResponse : Response { }
}