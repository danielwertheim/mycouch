using System;
using System.Net.Http;

namespace MyCouch.Net
{
    [Serializable]
    public class JsonContent : StringContent
    {
        public JsonContent(string content) 
            : base(content, MyCouchRuntime.DefaultEncoding, HttpContentTypes.Json) {}
    }
}