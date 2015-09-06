using MyCouch.Responses;
using System;

namespace MyCouch.Cloudant.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class IndexResponse : Response
    {
        public string Result { get; set; }
    }
}
