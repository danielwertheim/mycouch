using System;
using System.Globalization;
using System.Text;

namespace MyCouch
{
    public static class MyCouchRuntime
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public static readonly IFormatProvider GenericFormat = CultureInfo.InvariantCulture;
        public static readonly NumberFormatInfo NumberFormat = CultureInfo.InvariantCulture.NumberFormat;
        public static readonly string DateTimeFormatPattern = "yyyy-MM-dd HH:mm:ss";
    }
}
