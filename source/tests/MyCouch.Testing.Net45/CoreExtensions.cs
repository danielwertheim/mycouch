using System;
using System.Collections.Generic;

namespace MyCouch.Testing
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> src, Action<T> a)
        {
            foreach (var i in src)
                a(i);
        }

        public static void Each<T>(this IEnumerable<T> src, Action<int, T> a)
        {
            var i = 0;
            foreach (var item in src)
                a(i++, item);
        }
    }

    public static class StringExtensions
    {
        public static Uri ToTestUriFromRelative(this string value)
        {
            return new Uri(new Uri("http://localhost:5984"), value);
        }

        public static string ToUnescapedQuery(this string value)
        {
            return Uri.UnescapeDataString(value.ToTestUriFromRelative().Query);
        }
    }
}