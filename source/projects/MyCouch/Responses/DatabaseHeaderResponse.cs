using System;

namespace MyCouch.Responses
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class DatabaseHeaderResponse : Response
    {
        public string DbName { get; set; }
    }
}