using System.Globalization;

namespace Sisa.Panel.Extensions
{
    internal static class DateExtensions
    {
        static readonly string[] formats = ["dd.MM.yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss"];

        extension (string dateText)
        {
            public DateOnly ParseToDateOnly()
            {
                foreach (var format in formats)
                {
                    if (DateOnly.TryParseExact(dateText, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly result))
                        return result;
                }

                return DateOnly.TryParse(dateText, out DateOnly defaultResult) ? defaultResult : DateOnly.MinValue;
            }

            public DateTime ParseToDateTime()
            {
                foreach (var format in formats)
                {
                    if (DateTime.TryParseExact(dateText, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        return result;
                }

                return DateTime.TryParse(dateText, out DateTime defaultResult) ? defaultResult : DateTime.MinValue;
            }
        }
    }
}
