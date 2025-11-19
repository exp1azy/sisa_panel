using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using System.Globalization;

namespace Sisa.Panel.Parsers
{
    internal class SearchParser(IBrowsingContext context) : IParser<IReadOnlyList<PlayerSearchEntry>>
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

                if (cells.Count <= 1)
                    return entries.AsReadOnly();

                var entry = new PlayerSearchEntry
                {
                    Country = cells[1].ExtractImgAltAttribute()
                };

                var link = cells[1].QuerySelector("a");
                entry.Uid = link.ExtractUid();
                entry.Name = link.TextContent.Trim();

                var rankSpan = cells[2].QuerySelector("span[title='Rank']");
                entry.Rank = rankSpan?.TextContent ?? "N/A";

                var levelSpan = cells[3].QuerySelector("span.lvlx");
                var levelText = levelSpan.TextContent;
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
                entry.Online = onlineSpan?.TextContent ?? "Unknown";

                if (entry != null)
                    entries.Add(entry);
            }

            return entries.AsReadOnly();
        }

        private static string GetSpanTitleValue(IElement cell, string title)
        {
            var span = cell.QuerySelector($"span[title='{title}']");
            return span?.TextContent ?? cell.TextContent;
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
