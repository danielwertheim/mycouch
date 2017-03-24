using System;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public class IndexResponse : Response
    {
        public string Result { get; set; }
    }
}
