using System;

namespace MyCouch.Responses
{
#if !PCL
    [Serializable]
#endif
    public class DatabaseHeaderResponse : Response
    {
        public string DbName { get; set; }
    }
}