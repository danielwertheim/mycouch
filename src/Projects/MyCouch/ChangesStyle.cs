using System;
using EnsureThat;

namespace MyCouch
{
    public class ChangesStyle : IEquatable<ChangesStyle>, IEquatable<string>
    {
        protected readonly string Value;

        private static readonly ChangesStyle AllDocsValue = new ChangesStyle("all_docs");
        private static readonly ChangesStyle MainOnlyValue = new ChangesStyle("main_only");

        public static ChangesStyle AllDocs { get { return AllDocsValue; } }
        public static ChangesStyle MainOnly { get { return MainOnlyValue; } }

        private ChangesStyle(string value)
        {
            Ensure.That(value, "value").IsNotNull();
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChangesStyle);
        }

        public bool Equals(ChangesStyle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public bool Equals(string other)
        {
            return string.Equals(Value, other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(ChangesStyle item)
        {
            return item.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}