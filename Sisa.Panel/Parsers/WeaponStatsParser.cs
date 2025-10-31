using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Parsers
{
    internal class WeaponStatsParser(IBrowsingContext context) : IParser<IReadOnlyList<WeaponStatsEntry>>
    {
        public async Task<IReadOnlyList<WeaponStatsEntry>> ParseAsync(string htmlContent)
        {
            var document = await context.OpenAsync(req => req.Content(htmlContent));

            var entries = new List<WeaponStatsEntry>();
            var table = document.QuerySelector("table.table.table-bordered.table-condensed.table-hover.table-responsive.sortable");

            if (table == null)
                return entries.AsReadOnly();

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length >= 12)
                {
                    var entry = new WeaponStatsEntry
                    {
                        RatingPosition = ParseInt(cells[0].GetTextContent())
                    };

                    var flagImg = cells[1].QuerySelector("img");
                    var src = flagImg.GetAttribute("alt") ?? "";
                    entry.Country = src.Trim();

                    var link = cells[1].QuerySelector("a");
                    entry.Name = link.GetTextContent();

                    entry.Shots = ParseInt(GetSpanTitleValue(cells[2], "Выстрелов"));
                    entry.Hits = ParseInt(GetSpanTitleValue(cells[3], "Попаданий"));

                    var progressDiv = cells[4].QuerySelector("div.taskProgress");
                    var accuracyText = progressDiv.GetTextContent();
                    _ = int.TryParse(accuracyText, out int accuracy);
                    entry.Accuracy = accuracy;

                    entry.ZmKills = ParseInt(GetSpanTitleValue(cells[5], "Убийств Зомби"));
                    entry.ZmDamage = ParseInt(GetSpanTitleValue(cells[6], "Урон (ЗМ)"));
                    entry.Assists = ParseInt(GetSpanTitleValue(cells[7], "Ассистов"));
                    entry.MVPs = ParseInt(GetSpanTitleValue(cells[8], "Лучший игрок"));
                    entry.Levels = ParseInt(GetSpanTitleValue(cells[9], "Уровней"));
                    entry.BossDamage = ParseInt(GetSpanTitleValue(cells[10], "Урон (босс)"));
                    entry.BossKills = ParseInt(GetSpanTitleValue(cells[11], "Убийств босса"));

                    if (entry != null)
                        entries.Add(entry);
                }
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
