using System;
using EnsureThat;

namespace MyCouch.Net
{
    public class UrlSegment
    {
        public string Value { get; private set; }

        public UrlSegment(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Value = Uri.EscapeDataString(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}