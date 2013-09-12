using System.Globalization;
using System.Text;

namespace MyCouch
{
    public static class MyCouchRuntime
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public static readonly NumberFormatInfo NumberFormat = CultureInfo.InvariantCulture.NumberFormat;
    }
}
