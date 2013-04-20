using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyCouch.Net
{
    [Serializable]
    public class HttpRequest : HttpRequestMessage
    {
        public HttpRequest(HttpMethod method, string url) : base(method, url)
        {
            Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));
        }

        public virtual void SetIfMatch(string rev)
        {
            Headers.TryAddWithoutValidation("If-Match", rev);
        }

        public virtual void SetContent(string content)
        {
            if(content != null)
                Content = new JsonContent(content);
        }
    }
}