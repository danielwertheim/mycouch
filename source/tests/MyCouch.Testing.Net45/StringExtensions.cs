using System;

namespace MyCouch.Testing
{
    public static class StringExtensions
    {
        public static Uri ToTestUriFromRelative(this string value)
        {
            return new Uri(new Uri("http://localhost:5984"), value);
        }
    }
}