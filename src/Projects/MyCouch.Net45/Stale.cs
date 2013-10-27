using System;
using EnsureThat;

namespace MyCouch
{
    public class Stale : IEquatable<Stale>, IEquatable<string>
    {
        protected readonly string Value;

        private static readonly Stale OkValue = new Stale("ok");
        private static readonly Stale UpdateAfterValue = new Stale("update_after");

        public static Stale Ok { get { return OkValue; } }
        public static Stale UpdateAfter { get { return UpdateAfterValue; } }

        private Stale(string value)
        {
            Ensure.That(value, "value").IsNotNull();
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Stale);
        }

        public bool Equals(Stale other)
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

        public static implicit operator string(Stale item)
        {
            return item == null ? null: item.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}