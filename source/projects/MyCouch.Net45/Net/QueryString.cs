using System.Linq;
using MyCouch.Extensions;

namespace MyCouch.Net
{
    public class QueryString
    {
        public string Value { get; private set; }

        public QueryString(UrlParams parameters)
        {
            var queryParameters = parameters
                .Where(p => p != null && p.HasValue)
                .Select(p => string.Concat(p.Key, "=", p.Value)).ToList();

            Value = queryParameters.Any()
                ? string.Join("&", queryParameters).PrependWith("?")
                : string.Empty;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}