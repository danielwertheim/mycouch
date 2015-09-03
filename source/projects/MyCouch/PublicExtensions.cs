using System.Collections.Generic;
using System.Linq;

namespace MyCouch
{
    public static class PublicExtensions
    {
        public static object[] ToCouchKeys<T>(this IEnumerable<T> values)
        {
            return values.Select(v => (object)v).ToArray();
        }
    }
}