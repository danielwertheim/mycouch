using System;

namespace MyCouch.Extensions
{
    internal static class ObjectExtensions
    {
        internal static T To<T>(this object value)
        {
            return (T) value;
        }

        internal static bool IsNumeric(this object value)
        {
            return
                value is short || value is int || value is long || value is ushort || value is uint || value is ulong ||
                value is float || value is double || value is decimal;
        }

        internal static string ToStringExtended(this object v)
        {
            if (v == null)
                return null;

            if (v is string)
                return v.ToString();

            if (v is DateTime)
            {
                var d = (DateTime) v;

                return d.ToString(MyCouchRuntime.DateTimeFormatPattern, MyCouchRuntime.FormatingCulture);
            }

            //DUE TO P*N*S IMPLEMENTATION IN THE BEAST WINRT, WE CAN NOT USE IConvertible
#if !PCL
            var c = v as IConvertible;

            return c != null
                ? c.ToString(MyCouchRuntime.FormatingCulture)
                : v.ToString();
#else
            if (v is byte)
            {
                var i = (byte)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is short)
            {
                var i = (short)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is int)
            {
                var i = (int)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is long)
            {
                var i = (long)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is float)
            {
                var i = (float)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is double)
            {
                var i = (double)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            if (v is decimal)
            {
                var i = (decimal)v;

                return i.ToString(MyCouchRuntime.FormatingCulture);
            }

            return v.ToString();
#endif
        }
    }
}
