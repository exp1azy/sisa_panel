using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Responses;
using System.Globalization;

namespace Sisa.Panel.Parsers
{
    internal partial class PlayerStatsParser(IBrowsingContext context) : BaseParser<PlayerStats>(context)
    {
        public override async Task<PlayerStats> ParseAsync(string html)
        {
            var document = await Context.OpenAsync(req => req.Content(html));

            var stats = new PlayerStats
            {
                Stats = []
            };

            var table = document.QuerySelector("table.table-bordered.table-condensed");

            if (table == null) 
                return stats;

            var rows = table.QuerySelectorAll("tbody tr");

            foreach (var row in rows)
            {
                var player = ParsePlayerRow(row);

                if (player != null)
                    stats.Stats.Add(player);
            }

            return stats;
        }

        private static PlayerStatEntry ParsePlayerRow(IElement row)
    {
            var cells = row.QuerySelectorAll("td").ToArray();
            if (cells.Length < 10) return null;

            var player = new PlayerStatEntry()
            {
                RatingPosition = ParseRatingPosition(cells[0]),
                Country = ParseCountry(cells[1]),
                Name = ParseName(cells[1]),
                Level = ParseLevel(cells[2]),
                Exp = ParseExp(cells[3]),
                ZombieKills = ParseZombieKills(cells[4]),
                Assists = ParseAssists(cells[5]),
                Deaths = ParseDeaths(cells[6]),
                KD = ParseKD(cells[7]),
                MVPs = ParseMVPs(cells[8]),
                Knife = ParseKnife(cells[9])
            };

            return player;
        }

        private static int ParseRatingPosition(IElement cell)
        {
            var positionStr = cell.TextContent?.Trim();
            _ = int.TryParse(positionStr, out int position);

            return position;
        }

        private static string ParseCountry(IElement cell)
        {
            var flagImg = cell.QuerySelector("img");
            var alt = flagImg?.GetAttribute("alt");

            return alt ?? "Unknown";
        }

        private static string ParseName(IElement cell)
        {
            var textContent = cell.TextContent?.Trim();
            var playerName = PlayerNameRegex().Replace(textContent, " ").Trim();

            return playerName;
        }

        private static int ParseLevel(IElement cell)
        {
            var levelSpan = cell.QuerySelector("span.lvlx");
            var levelText = levelSpan.TextContent?.Trim();
            _ = int.TryParse(levelText, out int level);
            
            return level;
        }

        private static int ParseExp(IElement cell)
        {
            var expSpan = cell.QuerySelector("span[title='EXP']");
            string? exp;

            if (expSpan != null)
                exp = expSpan.TextContent?.Trim();
            else
                exp = cell.TextContent?.Trim();

            _ = int.TryParse(exp, out int expValue);
            return expValue;
        }

        private static int ParseZombieKills(IElement cell)
        {
            var killsSpan = cell.QuerySelector("span[title*='Убийств ЗМ']");
            string? killsStr;

            if (killsSpan != null)
                killsStr = killsSpan.TextContent?.Trim();
            else
                killsStr = cell.TextContent?.Trim();

            _ = int.TryParse(killsStr, out int zombieKills);
            return zombieKills;
        }

        private static int ParseAssists(IElement cell)
        {
            var assistsSpan = cell.QuerySelector("span[title*='Ассистов']");
            string? assistsStr;

            if (assistsSpan != null)
                assistsStr = assistsSpan.TextContent?.Trim();
            else
                assistsStr = cell.TextContent?.Trim();

            _ = int.TryParse(assistsStr, out int assists);
            return assists;
        }

        private static int ParseDeaths(IElement cell)
        {
            var deathsSpan = cell.QuerySelector("span[title*='Смертей']");
            string? deathsStr;

            if (deathsSpan != null)
                deathsStr = deathsSpan.TextContent?.Trim();
            else
                deathsStr = cell.TextContent?.Trim();

            _ = int.TryParse(deathsStr, out int deaths);
           return deaths;
        }

        private static float ParseKD(IElement cell)
        {
            var kdSpan = cell.QuerySelector("span[title*='Соотношение']");
            string? kdStr;

            if (kdSpan != null)
                kdStr = kdSpan.TextContent?.Trim();
            else
                kdStr = cell.TextContent?.Trim();

            _ = float.TryParse(kdStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float kd);
            return kd;
        }

        private static int ParseMVPs(IElement cell)
        {
            var mvpsSpan = cell.QuerySelector("span[title*='Л.И.']");
            string? mvpsStr;

            if (mvpsSpan != null)
                mvpsStr = mvpsSpan.TextContent?.Trim();
            else
                mvpsStr = cell.TextContent?.Trim();

            _ = int.TryParse(mvpsStr, out int mvps);
            return mvps;
        }

        private static string ParseKnife(IElement cell)
        {
            var knifeImg = cell.QuerySelector("img");
            var title = knifeImg.GetAttribute("title");
            
            return title;
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
        private static partial System.Text.RegularExpressions.Regex PlayerNameRegex();
    }
}
