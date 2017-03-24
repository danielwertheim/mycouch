using System.IO;

namespace MyCouch.Extensions
{
    internal static class StreamExtensions
    {
        internal static string ReadAsString(this Stream content)
        {
            var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding);
            return reader.ReadToEnd().TrimEnd();
        }
    }
}