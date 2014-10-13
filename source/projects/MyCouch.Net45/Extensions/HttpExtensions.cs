using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyCouch.Extensions
{
    public static class HttpExtensions
    {
        public static string ExtractIdFromUri(this HttpRequestMessage request, bool skipLast)
        {
            var index = request.RequestUri.Segments.Length - (1 + (skipLast ? 1 : 0));

            var peek = request.RequestUri.Segments[index - 1];
            if (string.Equals(peek, "_design/", StringComparison.OrdinalIgnoreCase))
                return string.Concat(peek, request.RequestUri.Segments[index]);

            return Uri.UnescapeDataString(request.RequestUri.Segments[index].RemoveTrailing("/"));
        }

        public static string ExtractAttachmentNameFromUri(this HttpRequestMessage request)
        {
            return request.RequestUri.Segments.Any()
                ? Uri.UnescapeDataString(request.RequestUri.Segments.Last())
                : null;
        }

        public static string GetETag(this HttpResponseHeaders headers)
        {
            return headers.ETag == null || headers.ETag.Tag == null
                ? string.Empty
                : headers.ETag.Tag.Substring(1, headers.ETag.Tag.Length - 2);
        }
    }
}