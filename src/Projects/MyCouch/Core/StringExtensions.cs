using System;
using System.Globalization;
using System.Text;

namespace MyCouch.Core
{
    public static class StringExtensions
    {
        public static string AsBase64Encoded(this string value)
        {
            return Convert.ToBase64String(MyCouchRuntime.DefaultEncoding.GetBytes(value));
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
                    sb.Append(char.ToLower(s[i], CultureInfo.InvariantCulture));
                    continue;
                }
                sb.Append(s.Substring(i));
                break;
            }

            return sb.ToString();
        } 
    }
}