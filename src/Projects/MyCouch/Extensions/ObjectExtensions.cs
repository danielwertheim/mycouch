using System;

namespace MyCouch.Extensions
{
    public static class ObjectExtensions
    {
        public static T To<T>(this object value)
        {
            return (T) value;
        }

        public static bool IsNumeric(this object value)
        {
            return value is IConvertible && (
                value is short || value is int || value is long || value is ushort || value is uint || value is ulong ||
                value is float || value is double || value is decimal);
        }

        public static bool IsBool(this object value)
        {
            return value is bool;
        }

        public static bool IsDateTime(this object value)
        {
            return value is DateTime;
        }
    }
}
