using EnsureThat;

namespace MyCouch
{
    public class Stale
    {
        protected readonly string Value;

        private Stale(string value)
        {
            Ensure.That(value, "value").IsNotNull();
            Value = value;
        }

        public static implicit operator string(Stale item)
        {
            return item.Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static Stale Ok { get { return new Stale("ok"); } }
        public static Stale UpdateAfter { get { return new Stale("update_after"); } }
    }
}