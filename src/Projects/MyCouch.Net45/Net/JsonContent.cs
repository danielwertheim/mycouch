using System;
using System.Net.Http;

namespace MyCouch.Net
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class JsonContent : StringContent
    {
        public JsonContent()
            : base(string.Empty, MyCouchRuntime.DefaultEncoding, HttpContentTypes.Json) { }

        public JsonContent(string content) 
            : base(content, MyCouchRuntime.DefaultEncoding, HttpContentTypes.Json) {}
    }
}