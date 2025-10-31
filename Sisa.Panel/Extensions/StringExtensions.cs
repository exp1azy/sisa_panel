using System.Globalization;

namespace Sisa.Panel.Extensions
{
    internal static class StringExtensions
    {
        public static DateOnly ParseToDateOnly(this string dateText)
        {
            _ = DateOnly.TryParseExact(dateText, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date);
            return date;
        }

        public static DateTime ParseToDateTime(this string dateText)
        {
            _ = DateTime.TryParseExact(dateText, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            return date;
        }
    }
}
