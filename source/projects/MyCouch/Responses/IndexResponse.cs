using System;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class IndexResponse : Response
    {
        public string Result { get; set; }
    }
}
