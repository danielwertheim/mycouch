using System;
using EnsureThat;

namespace MyCouch.Net
{
#if !PCL
    [Serializable]
#endif
    public class UrlParam
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        public bool HasValue { get { return !string.IsNullOrWhiteSpace(Value); } }

        public UrlParam(string key, string value = null)
        {
            Ensure.That(key, "key").IsNotNullOrWhiteSpace();

            Key = key;
            Value = value;
        }
    }
}