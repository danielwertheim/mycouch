using System;

namespace MyCouch
{
    public static class Now
    {
        public static Func<DateTime> ValueGenerator { private get; set; }

        public static DateTime Value
        {
            get { return ValueGenerator(); }
        }

        static Now()
        {
            Reset();
        }

        public static void Reset()
        {
            ValueGenerator = () => DateTime.Now;
        }
    }
}