using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using System.Globalization;

namespace Sisa.Panel.Parsers
{
    internal class PlayerStatsParser(IBrowsingContext context) : IParser<IReadOnlyList<PlayerStatEntry>>
    {
        public async Task<IReadOnlyList<PlayerStatEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var stats = new List<PlayerStatEntry>();

            var table = document.QuerySelector("table.table-bordered.table-condensed");
            if (table == null)
                return [];

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length < 10) continue;

                var player = new PlayerStatEntry()
                {
                    RatingPosition = ParseRatingPosition(cells[0]),
                    Uid = ParseUid(cells[1]),
                    Country = ParseCountry(cells[1]),
                    Name = ParseName(cells[1]),
                    Level = ParseLevel(cells[2]),
                    Exp = ParseExp(cells[3]),
                    ZmKills = ParseZombieKills(cells[4]),
                    Assists = ParseAssists(cells[5]),
                    Deaths = ParseDeaths(cells[6]),
                    KillDeathRatio = ParseKD(cells[7]),
                    MVPs = ParseMVPs(cells[8]),
                    Knife = ParseKnife(cells[9])
                };

                if (player != null)
                    stats.Add(player);
            }

            return stats;
        }

        private static int ParseRatingPosition(IElement cell)
        {
            var positionStr = cell.GetTextContent();
            _ = int.TryParse(positionStr, out int position);

            return position;
        }

        private static int ParseUid(IElement cell)
        {
            var link = cell.QuerySelector("a[href*='uid=']");
            if (link != null)
            {
                var href = link.GetAttribute("href") ?? "";
                var uidMatch = ParserRegex.UidPattern().Match(href);

                if (uidMatch.Success && int.TryParse(uidMatch.Groups[1].Value, out int uid))
                    return uid;
            }

            return 0;
        }

        private static string ParseCountry(IElement cell)
        {
            var flagImg = cell.QuerySelector("img");
            var alt = flagImg?.GetAttribute("alt");

            return alt ?? "Unknown";
        }

        private static string ParseName(IElement cell)
        {
            var textContent = cell.GetTextContent();
            return ParserRegex.WhitespaceCleanupPattern().Replace(textContent, " ").Trim();
        }

        private static int ParseLevel(IElement cell)
        {
            var levelSpan = cell.QuerySelector("span.lvlx");
            var levelText = levelSpan.GetTextContent();
            _ = int.TryParse(levelText, out int level);
            
            return level;
        }

        private static int ParseExp(IElement cell)
        {
            var expSpan = cell.QuerySelector("span[title='EXP']");
            string? exp;

            if (expSpan != null)
                exp = expSpan.GetTextContent();
            else
                exp = cell.GetTextContent();

            _ = int.TryParse(exp, out int expValue);
            return expValue;
        }

        private static int ParseZombieKills(IElement cell)
        {
            var killsSpan = cell.QuerySelector("span[title*='Убийств ЗМ']");
            string? killsStr;

            if (killsSpan != null)
                killsStr = killsSpan.GetTextContent();
            else
                killsStr = cell.GetTextContent();

            _ = int.TryParse(killsStr, out int zombieKills);
            return zombieKills;
        }

        private static int ParseAssists(IElement cell)
        {
            var assistsSpan = cell.QuerySelector("span[title*='Ассистов']");
            string? assistsStr;

            if (assistsSpan != null)
                assistsStr = assistsSpan.GetTextContent();
            else
                assistsStr = cell.GetTextContent();

            _ = int.TryParse(assistsStr, out int assists);
            return assists;
        }

        private static int ParseDeaths(IElement cell)
        {
            var deathsSpan = cell.QuerySelector("span[title*='Смертей']");
            string? deathsStr;

            if (deathsSpan != null)
                deathsStr = deathsSpan.GetTextContent();
            else
                deathsStr = cell.GetTextContent();

            _ = int.TryParse(deathsStr, out int deaths);
           return deaths;
        }

        private static float ParseKD(IElement cell)
        {
            var kdSpan = cell.QuerySelector("span[title*='Соотношение']");
            string? kdStr;

            if (kdSpan != null)
                kdStr = kdSpan.GetTextContent();
            else
                kdStr = cell.GetTextContent();

            _ = float.TryParse(kdStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float kd);
            return kd;
        }

        private static int ParseMVPs(IElement cell)
        {
            var mvpsSpan = cell.QuerySelector("span[title*='Л.И.']");
            string? mvpsStr;

            if (mvpsSpan != null)
                mvpsStr = mvpsSpan.GetTextContent();
            else
                mvpsStr = cell.GetTextContent();

            _ = int.TryParse(mvpsStr, out int mvps);
            return mvps;
        }

        private static string ParseKnife(IElement cell)
        {
            var knifeImg = cell.QuerySelector("img");
            return knifeImg.GetAttribute("title");
        }
    }
}
