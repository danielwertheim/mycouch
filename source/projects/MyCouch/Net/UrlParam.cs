using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class UrlParam
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        public bool HasValue { get { return !string.IsNullOrWhiteSpace(Value); } }

        public UrlParam(string key, string value = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(key, nameof(key));

            Key = key;

            if (value != null)
                Value = Uri.EscapeDataString(value);
        }

        public override string ToString()
        {
            return string.Concat(Key, "=", Value);
        }

        public static string Encode(string value)
        {
            return Uri.EscapeDataString(value);
        }
    }
}