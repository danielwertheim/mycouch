using System;
using System.Text;

namespace MyCouch.Extensions
{
    public static class StringExtensions
    {
        public static string AsBase64Encoded(this string value)
        {
            return Convert.ToBase64String(MyCouchRuntime.DefaultEncoding.GetBytes(value));
        }

        public static string PrependWith(this string value, string prefix)
        {
            return string.Concat(prefix, value);
        }

        public static string RemoveStarting(this string value, string starting)
        {
            while (value.StartsWith(starting))
                value = value.Substring(starting.Length);

            return value;
        }

        public static string RemoveTrailing(this string value, string ending)
        {
            while (value.EndsWith(ending))
                value = value.Substring(0, value.Length - ending.Length);

            return value;
        }

        public static string TrimToEnd(this string value, string shouldEndWith)
        {
            if (value.EndsWith(shouldEndWith))
                return value;

            return value.Substring(0, value.IndexOf(shouldEndWith, StringComparison.OrdinalIgnoreCase));
        }

        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            if (!char.IsUpper(s[0]))
                return s;

            var sb = new StringBuilder();
            var lastIndex = s.Length - 1;
            for (var i = 0; i < s.Length; i++)
            {
                if (i == 0 || i == lastIndex || char.IsUpper(s[i + 1]))
                {
                    sb.Append(char.ToLower(s[i]));
                    continue;
                }
                sb.Append(s.Substring(i));
                break;
            }

            return sb.ToString();
        }
    }
}