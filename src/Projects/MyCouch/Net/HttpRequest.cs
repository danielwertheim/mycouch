using System;
using System.Net.Http;
using System.Net.Http.Headers;
using EnsureThat;

namespace MyCouch.Net
{
    [Serializable]
    public class HttpRequest : HttpRequestMessage
    {
        public HttpRequest(HttpMethod method, string url) : base(method, new Uri(url))
        {
            Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));
        }

        public virtual void SetIfMatch(string rev)
        {
            if(!string.IsNullOrWhiteSpace(rev))
                Headers.TryAddWithoutValidation("If-Match", rev);
        }

        public virtual void SetContent(string content)
        {
            if(!string.IsNullOrWhiteSpace(content))
                Content = new JsonContent(content);
        }
    }
}