using System;
using System.Linq;

namespace MyCouch
{
public static class UriMagic
{
    public static Uri Abracadabra(params string[] parts)
        => new Uri(string.Join(
            "/",
            parts.Select(p => p.Trim(' ', '/'))));
}
}