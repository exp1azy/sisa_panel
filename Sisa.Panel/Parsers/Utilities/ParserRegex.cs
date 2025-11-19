using System.Text.RegularExpressions;

namespace Sisa.Panel.Parsers
{
    internal static partial class ParserRegex
    {
        [GeneratedRegex(@"Всего банов:\s*(\d+)\s*\((\d+)\s*active\)", RegexOptions.IgnoreCase)]
        public static partial Regex TotalBansPattern();

        [GeneratedRegex(@"Всего демо в базе данных\s*:\s*(\d+)")]
        public static partial Regex TotalDemosPattern();

        [GeneratedRegex(@"\s+")]
        public static partial Regex WhitespaceCleanupPattern();

        [GeneratedRegex(@"[?&]id=(\d+)")]
        public static partial Regex UrlIdExtractorPattern();

        [GeneratedRegex(@"wid=(\d+)")]
        public static partial Regex WidPattern();

        [GeneratedRegex(@"(\d+)\s*/\s*(\d+)")]
        public static partial Regex TimeLeftPattern();

        [GeneratedRegex(@"(\d+)\s+игрок(а|ов)?")]
        public static partial Regex PlayerCountPattern();

        [GeneratedRegex(@"(\d+)\s*:\s*(\d+)")]
        public static partial Regex FragsPattern();

        [GeneratedRegex(@"{x:\s*'([^']+)',\s*y:\s*(\d+)}")]
        public static partial Regex ActivityPattern();

        [GeneratedRegex(@"/(7656119\d+)/")]
        public static partial Regex SteamIdPattern();

        [GeneratedRegex(@"^(.+?)\s*\((.+?)\)$")]
        public static partial Regex TempWeaponPattern();

        [GeneratedRegex(@"^.*?\s+\&nbsp;")]
        public static partial Regex TrimUntilNbspPattern();

        [GeneratedRegex(@"(\d+(?:\s?\d+)*)\s+осталось")]
        public static partial Regex RemainingTimePattern();

        [GeneratedRegex(@"(\d+(?:\s?\d+)*)")]
        public static partial Regex FormattedNumberExtractorPattern();

        [GeneratedRegex(@"uid=(\d+)")]
        public static partial Regex UidPattern();
    }
}
