using System;
using System.Net.Http;

namespace MyCouch.Net
{
#if !WinRT
    [Serializable]
#endif
    public class JsonContent : StringContent
    {
        public JsonContent(string content) 
            : base(content, MyCouchRuntime.DefaultEncoding, HttpContentTypes.Json) {}
    }
}