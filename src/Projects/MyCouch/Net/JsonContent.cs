using System;
using System.Net.Http;
using System.Text;

namespace MyCouch.Net
{
    [Serializable]
    public class JsonContent : StringContent
    {
        public JsonContent(string content) 
            : base(content, Encoding.UTF8, "application/json") {}
    }
}