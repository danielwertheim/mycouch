using System.Globalization;
using System.Text;

namespace MyCouch
{
    public static class MyCouchRuntime
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public static readonly CultureInfo FormatingCulture = CultureInfo.InvariantCulture;
        public static readonly string DateTimeFormatPattern = "yyyy-MM-ddTHH:mm:ss";
    }
}
