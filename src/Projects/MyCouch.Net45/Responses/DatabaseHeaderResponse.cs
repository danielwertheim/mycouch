using System;

namespace MyCouch.Responses
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class DatabaseHeaderResponse : Response
    {
        public string DbName { get; set; }
    }
}