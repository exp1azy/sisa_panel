using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using System.Globalization;

namespace Sisa.Panel.Parsers
{
    internal class PlayerSearchParser(IBrowsingContext context) : IParser<IReadOnlyList<PlayerSearchEntry>>
    {
        public async Task<IReadOnlyList<PlayerSearchEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var entries = new List<PlayerSearchEntry>();

            var table = document.QuerySelector("table.table.table-bordered.table-condensed.table-hover.table-responsive.sortable");
            if (table == null)
                return entries.AsReadOnly();

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                var entry = new PlayerSearchEntry();

                var flagImg = cells[1].QuerySelector("img");
                entry.Country = flagImg.GetAttribute("alt") ?? "";

                var link = cells[1].QuerySelector("a");
                var name = link.GetTextContent();
                name = ParserRegex.WhitespaceCleanupPattern().Replace(name, " ").Trim();
                entry.Name = name;

                var rankSpan = cells[2].QuerySelector("span[title='Rank']");
                entry.Rank = rankSpan?.GetTextContent() ?? "N/A";

                var levelSpan = cells[3].QuerySelector("span.lvlx");
                var levelText = levelSpan.GetTextContent();
                _ = int.TryParse(levelText, out int level);
                entry.Level = level;

                entry.Exp = ParseInt(GetSpanTitleValue(cells[4], "EXP"));
                entry.ZmKills = ParseInt(GetSpanTitleValue(cells[5], "Убийств ЗМ"));
                entry.Assists = ParseInt(GetSpanTitleValue(cells[6], "Ассистов"));
                entry.Deaths = ParseInt(GetSpanTitleValue(cells[7], "Смертей"));

                var ratioText = GetSpanTitleValue(cells[8], "Соотношение убийств / смертей");
                _ = decimal.TryParse(ratioText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ratio);
                entry.KillDeathRatio = ratio;

                entry.MVPs = ParseInt(GetSpanTitleValue(cells[9], "Л.И."));

                var onlineSpan = cells[10].QuerySelector("span[title='Онлайн']");
                entry.Online = onlineSpan?.GetTextContent() ?? "Unknown";

                if (entry != null)
                    entries.Add(entry);
            }

            return entries.AsReadOnly();
        }

        private static string GetSpanTitleValue(IElement cell, string title)
        {
            var span = cell.QuerySelector($"span[title='{title}']");
            return span?.GetTextContent() ?? cell.GetTextContent();
        }

        private static int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Replace(" ", "").Replace(",", "");

            if (int.TryParse(value, out int result))
                return result;

            return 0;
        }
    }
}
