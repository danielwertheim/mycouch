using System;

namespace MyCouch.Responses
{
#if net45
    [Serializable]
#endif
    public class DatabaseHeaderResponse : Response
    {
        public string DbName { get; set; }
    }
}