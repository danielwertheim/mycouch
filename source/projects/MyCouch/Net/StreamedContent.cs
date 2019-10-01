using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyCouch.Net
{
    public class StreamedContent : StreamContent
    {
        public StreamedContent(Stream content, string contentType) : base(content)
        {
            Headers.ContentType = new MediaTypeHeaderValue(contentType);
        }
    }
}