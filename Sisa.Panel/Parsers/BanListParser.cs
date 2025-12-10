using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.BanList;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class BanListParser(IBrowsingContext context) : IParser<BanList>
    {
        public async Task<BanList> ParseAsync(string htmlContent)
        {
            var document = await context.OpenAsync(req => req.Content(htmlContent));

            var banList = new BanList
            {
                Bans = ParseBansTable(document)
            };

            var summaryRow = document.QuerySelector("td[colspan]");
            if (summaryRow != null)
            {
                var summaryText = summaryRow.TextContent;

                var bansMatch = ParserRegex.TotalBansPattern.Match(summaryText);
                if (bansMatch.Success)
                {
                    banList.TotalBans = int.Parse(bansMatch.Groups[1].Value);
                    banList.ActiveBans = int.Parse(bansMatch.Groups[2].Value);
                }

                var demosMatch = ParserRegex.TotalDemosPattern.Match(summaryText);
                if (demosMatch.Success)
                    banList.TotalDemos = int.Parse(demosMatch.Groups[1].Value);
            }

            return banList;
        }

        private static List<BanEntry> ParseBansTable(IDocument document)
        {
            var table = document.QuerySelector("table.table");

            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var bans = new List<BanEntry>(rows.Length);

            foreach (var row in rows)
            {
                if (row.QuerySelector("td[colspan]") != null)
                    continue;

                var cols = row.GetTableCells();
                if (cols.Length < 7)
                    return bans;

                var dataTarget = row.GetAttribute("data-target");

                int banId = 0;
                if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWithOrdinal("#ban-"))
                {
                    var idStr = dataTarget[5..];
                    _ = int.TryParse(idStr, out banId);
                }

                var content = cols[0].InnerHtml;
                string banType = "unknown";

                if (content.ContainsOrdinal("internet-explorer"))
                    banType = "website";
                else if (content.ContainsOrdinal("czero.gif"))
                    banType = "czero";

                var ban = new BanEntry
                {
                    Id = banId,
                    Date = cols[1].TextContent.Trim(),
                    PlayerName = ExtractPlayerName(cols[2]),
                    Country = cols[2].ExtractImgAltAttribute(),
                    AdminName = cols[3].TextContent.Trim(),
                    Reason = cols[4].TextContent,
                    Duration = cols[5].TextContent,
                    HasDemo = !cols[6].TextContent.ContainsOrdinal("нет демо"),
                    Client = banType
                };

                if (ban != null)
                {
                    var modal = document.QuerySelector($"#ban-{banId}");
                    if (modal == null)
                        return bans;

                    var modalData = new BanEntry();

                    var steamIdRow = modal.QuerySelector(".row-fluid:has(.span5:contains('ID Номер'))");
                    if (steamIdRow != null)
                    {
                        var steamIdCell = steamIdRow.QuerySelector(".span6");
                        modalData.SteamId = steamIdCell.TextContent.Trim();
                    }

                    var steamProfileRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Steam профиль'))");
                    if (steamProfileRow != null)
                    {
                        var steamProfileCell = steamProfileRow.QuerySelector(".span6");
                        modalData.SteamProfile = ExtractSteamProfile(steamProfileCell);
                        modalData.IsSteamUser = modalData.SteamProfile != "Non Steam";
                    }

                    var expiresRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Истекает'))");
                    if (expiresRow != null)
                    {
                        var expiresCell = expiresRow.QuerySelector(".span6");
                        modalData.ExpiresDate = expiresCell.TextContent.Trim();
                    }

                    var violationsRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Предыдущих нарушений'))");
                    if (violationsRow != null)
                    {
                        var violationsCell = violationsRow.QuerySelector(".span6");
                        if (int.TryParse(violationsCell.TextContent, out int violations))
                            modalData.PreviousViolations = violations;
                    }

                    var serverRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Забанен на сервере'))");
                    if (serverRow != null)
                    {
                        var serverCell = serverRow.QuerySelector(".span6");
                        modalData.Server = serverCell.TextContent.Trim();
                    }

                    if (modalData != null)
                    {
                        ban.SteamId = modalData.SteamId;
                        ban.ExpiresDate = modalData.ExpiresDate;
                        ban.PreviousViolations = modalData.PreviousViolations;
                        ban.Server = modalData.Server;
                        ban.IsSteamUser = modalData.IsSteamUser;
                        ban.SteamProfile = modalData.SteamProfile;
                    }

                    bans.Add(ban);
                }
            }

            return bans;
        }

        private static string ExtractPlayerName(IElement playerCell)
        {
            var textNodes = playerCell.ChildNodes
                .Where(n => n.NodeType == NodeType.Text)
                .Select(n => n.TextContent.Trim())
                .Where(t => !string.IsNullOrEmpty(t));

            return string.Join(" ", textNodes).Trim();
        }

        private static string ExtractSteamProfile(IElement steamProfileCell)
        {
            if (steamProfileCell == null)
                return "Unknown";

            var steamIcon = steamProfileCell.QuerySelector("i.fa-steam");
            if (steamIcon != null)
            {
                var steamLink = steamProfileCell.GetSteamProfileElement();

                if (steamLink != null)
                    return steamLink.GetAttribute("href");

                return "Steam User (No link)";
            }

            var nonSteamIcon = steamProfileCell.QuerySelector("i.fa-times");

            if (nonSteamIcon != null)
                return "Non Steam";

            return "Unknown";
        }
    }
}