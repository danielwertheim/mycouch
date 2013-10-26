using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyCouch.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUriSegmentByRightOffset(this HttpRequestMessage request, int offset = 0)
        {
            var segments = request.RequestUri.Segments;
            var val = segments[segments.Length - (1 + offset)];

            return val.EndsWith("/")
                ? val.Substring(0, val.Length - 1)
                : val;
        }

        public static string GetETag(this HttpResponseHeaders headers)
        {
            return headers.ETag == null || headers.ETag.Tag == null
                ? string.Empty
                : headers.ETag.Tag.Substring(1, headers.ETag.Tag.Length - 2);
        }

        public static Stream ReadAsStream(this HttpContent content)
        {
            return content.ReadAsStreamAsync().Result;
        }
    }
}