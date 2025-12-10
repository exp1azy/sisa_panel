using System.Text.RegularExpressions;

namespace Sisa.Panel.Parsers
{
    internal static partial class ParserRegex
    {
        public static readonly Regex TotalBansPattern = TotalBansGeneratedPattern();
        public static readonly Regex TotalDemosPattern = TotalDemosGeneratedPattern();
        public static readonly Regex WhitespaceCleanupPattern = WhitespaceCleanupGeneratedPattern();
        public static readonly Regex UrlIdExtractorPattern = UrlIdExtractorGeneratedPattern();
        public static readonly Regex WidPattern = WidGeneratedPattern();
        public static readonly Regex TimeLeftPattern = TimeLeftGeneratedPattern();
        public static readonly Regex PlayerCountPattern = PlayerCountGeneratedPattern();
        public static readonly Regex FragsPattern = FragsGeneratedPattern();      
        public static readonly Regex ActivityPattern = ActivityGeneratedPattern();
        public static readonly Regex SteamIdPattern = SteamIdGeneratedPattern();
        public static readonly Regex TempWeaponPattern = TempWeaponGeneratedPattern();
        public static readonly Regex TrimUntilNbspPattern = TrimUntilNbspGeneratedPattern();
        public static readonly Regex RemainingTimePattern = RemainingTimeGeneratedPattern();
        public static readonly Regex FormattedNumberExtractorPattern = FormattedNumberExtractorGeneratedPattern();
        public static readonly Regex UidPattern = UidGeneratedPattern();

        [GeneratedRegex(@"Всего банов:\s*(\d+)\s*\((\d+)\s*active\)", RegexOptions.IgnoreCase)]
        private static partial Regex TotalBansGeneratedPattern();

        [GeneratedRegex(@"Всего демо в базе данных\s*:\s*(\d+)")]
        private static partial Regex TotalDemosGeneratedPattern();

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceCleanupGeneratedPattern();

        [GeneratedRegex(@"[?&]id=(\d+)")]
        private static partial Regex UrlIdExtractorGeneratedPattern();

        [GeneratedRegex(@"wid=(\d+)")]
        private static partial Regex WidGeneratedPattern();

        [GeneratedRegex(@"(\d+)\s*/\s*(\d+)")]
        private static partial Regex TimeLeftGeneratedPattern();

        [GeneratedRegex(@"(\d+)\s+игрок(а|ов)?")]
        private static partial Regex PlayerCountGeneratedPattern();

        [GeneratedRegex(@"(\d+)\s*:\s*(\d+)")]
        private static partial Regex FragsGeneratedPattern();

        [GeneratedRegex(@"{x:\s*'([^']+)',\s*y:\s*(\d+)}")]
        private static partial Regex ActivityGeneratedPattern();

        [GeneratedRegex(@"/(7656119\d+)/")]
        private static partial Regex SteamIdGeneratedPattern();

        [GeneratedRegex(@"^(.+?)\s*\((.+?)\)$")]
        private static partial Regex TempWeaponGeneratedPattern();

        [GeneratedRegex(@"^.*?\s+\&nbsp;")]
        private static partial Regex TrimUntilNbspGeneratedPattern();

        [GeneratedRegex(@"(\d+(?:\s?\d+)*)\s+осталось")]
        private static partial Regex RemainingTimeGeneratedPattern();

        [GeneratedRegex(@"(\d+(?:\s?\d+)*)")]
        private static partial Regex FormattedNumberExtractorGeneratedPattern();

        [GeneratedRegex(@"uid=(\d+)")]
        private static partial Regex UidGeneratedPattern();
    }
}
