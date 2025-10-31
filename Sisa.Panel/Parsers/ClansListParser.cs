using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Clans;

namespace Sisa.Panel.Parsers
{
    internal partial class ClanListParser(IBrowsingContext context) : IParser<IReadOnlyList<ClanEntry>>
    {
        public async Task<IReadOnlyList<ClanEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var clans = new List<ClanEntry>();

            var table = document.QuerySelector("table.table-bordered");
            if (table == null)
                return clans;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length < 4)
                    return null;

                var clan = new ClanEntry
                {
                    Rating = 0
                };

                var fullStars = cells[0].QuerySelectorAll("i.fa-star:not(.fa-star-half-o)");
                clan.Rating = fullStars.Length;
                var halfStars = cells[0].QuerySelectorAll("i.fa-star-half-o");
                clan.Rating += halfStars.Length * 0.5f;

                var diamondIcon = cells[1].QuerySelector("i.fa-diamond");
                clan.InTop = diamondIcon != null;
                var clanLink = cells[1].QuerySelector("a");

                if (clanLink != null)
                {
                    clan.ClanName = clanLink.GetTextContent();

                    var href = clanLink.GetAttribute("href");
                    var idMatch = IdRegex().Match(href);

                    if (idMatch.Success && idMatch.Groups.Count > 1)
                    {
                        _ = int.TryParse(idMatch.Groups[1].Value, out int id);
                        clan.Id = id;
                    }
                }
                else
                {
                    clan.ClanName = cells[1].GetTextContent();
                    clan.Id = -1;
                }

                if (!string.IsNullOrEmpty(clan.ClanName))
                    clan.ClanName = ClanNameRegex().Replace(clan.ClanName, " ").Trim();

                var actions = new List<string>();

                foreach (var span in cells[2].QuerySelectorAll("span"))
                {
                    var title = span.GetAttribute("title")?.Trim();

                    if (string.IsNullOrEmpty(title))
                        continue;

                    actions.Add(title);
                }

                clan.Actions = actions;

                var playersText = cells[3].GetTextContent();

                if (int.TryParse(playersText, out int playersCount))
                    clan.PlayersCount = playersCount;
                else
                    clan.PlayersCount = 0;

                if (clan != null)
                    clans.Add(clan);
            }

            return clans;
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
        private static partial System.Text.RegularExpressions.Regex ClanNameRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"[?&]id=(\d+)")]
        private static partial System.Text.RegularExpressions.Regex IdRegex();
    }
}