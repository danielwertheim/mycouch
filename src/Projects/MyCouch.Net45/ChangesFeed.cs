using System;
using EnsureThat;

namespace MyCouch
{
    public class ChangesFeed : IEquatable<ChangesFeed>, IEquatable<string>
    {
        protected readonly string Value;

        private static readonly ChangesFeed NormalValue = new ChangesFeed("normal");
        private static readonly ChangesFeed LongpollValue = new ChangesFeed("longpoll");
        private static readonly ChangesFeed ContinuousValue = new ChangesFeed("continuous");

        public static ChangesFeed Normal { get { return NormalValue; } }
        public static ChangesFeed Longpoll { get { return LongpollValue; } }
        public static ChangesFeed Continuous { get { return ContinuousValue; } }

        private ChangesFeed(string value)
        {
            Ensure.That(value, "value").IsNotNull();
            Value = value;
        }

        public static bool operator ==(ChangesFeed left, ChangesFeed right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChangesFeed left, ChangesFeed right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChangesFeed);
        }

        public bool Equals(ChangesFeed other)
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

        public static implicit operator string(ChangesFeed item)
        {
            return item.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}