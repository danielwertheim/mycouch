using System;
using System.Linq;

namespace MyCouch.Extensions
{
    public static class UriExtensions
    {
        public static string ExtractDbName(this Uri value)
        {
            return value.LocalPath.TrimStart('/').TrimEnd('/', '?').Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }
    }
}