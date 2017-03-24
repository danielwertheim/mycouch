using System;

namespace MyCouch.Extensions
{
    internal static class ObjectExtensions
    {
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

            //No IConvertible
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
        }
    }
}
