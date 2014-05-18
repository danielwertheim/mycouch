using System;
using System.Linq;
using MyCouch.Net;

namespace MyCouch.Extensions
{
    public static class UriExtensions
    {
        public static string GetAbsoluteUriExceptUserInfo(this Uri uri)
        {
            return uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped);
        }

        public static BasicAuthString GetBasicAuthString(this Uri uri)
        {
            if (string.IsNullOrWhiteSpace(uri.UserInfo))
                return null;

            var parts = GetUserInfoParts(uri);

            return new BasicAuthString(parts[0], parts[1]);
        }

        public static string[] GetUserInfoParts(this Uri uri)
        {
            return uri.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => Uri.UnescapeDataString(p))
                .ToArray();
        }

        public static string ExtractDbName(this Uri value)
        {
            return value.LocalPath.TrimStart('/').TrimEnd('/', '?').Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }
    }
}