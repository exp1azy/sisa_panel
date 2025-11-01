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

                var bansMatch = ParserRegex.TotalBansPattern().Match(summaryText);
                if (bansMatch.Success)
                {
                    banList.TotalBans = int.Parse(bansMatch.Groups[1].Value);
                    banList.ActiveBans = int.Parse(bansMatch.Groups[2].Value);
                }

                var demosMatch = ParserRegex.TotalDemosPattern().Match(summaryText);
                if (demosMatch.Success)
                    banList.TotalDemos = int.Parse(demosMatch.Groups[1].Value);
            }

            return banList;
        }

        private static List<BanEntry> ParseBansTable(IDocument document)
        {
            var bans = new List<BanEntry>();

            var table = document.QuerySelector("table.table");
            if (table == null)
                return bans;

            foreach (var row in table.GetTableRows())
            {
                if (row.QuerySelector("td[colspan]") != null)
                    continue;

                var cols = row.GetTableCells();
                if (cols.Length < 7)
                    return null;

                var dataTarget = row.GetAttribute("data-target");

                int banId = 0;
                if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWith("#ban-"))
                {
                    var idStr = dataTarget[5..];
                    _ = int.TryParse(idStr, out banId);
                }

                var content = cols[0].InnerHtml;
                string banType = "unknown";

                if (content.Contains("internet-explorer"))
                    banType = "website";
                else if (content.Contains("cstrike.gif"))
                    banType = "cs";
                else if (content.Contains("czero.gif"))
                    banType = "czero";

                var ban = new BanEntry
                {
                    BanId = banId,
                    Date = cols[1].GetTextContent(),
                    PlayerName = ExtractPlayerName(cols[2]),
                    Country = ExtractCountry(cols[2]),
                    AdminName = cols[3].GetTextContent(),
                    Reason = cols[4].GetTextContent(),
                    Duration = cols[5].GetTextContent(),
                    HasDemo = !cols[6].TextContent.Contains("нет демо"),
                    BanType = banType
                };

                if (ban != null)
                {
                    var modal = document.QuerySelector($"#ban-{banId}");
                    if (modal == null)
                        return null;

                    var modalData = new BanEntry();

                    var steamIdRow = modal.QuerySelector(".row-fluid:has(.span5:contains('ID Номер'))");
                    if (steamIdRow != null)
                    {
                        var steamIdCell = steamIdRow.QuerySelector(".span6");
                        modalData.SteamId = steamIdCell.GetTextContent();
                    }

                    var steamProfileRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Steam профиль'))");
                    if (steamProfileRow != null)
                    {
                        var steamProfileCell = steamProfileRow.QuerySelector(".span6");
                        modalData.SteamProfile = ExtractSteamProfile(steamProfileCell);
                        modalData.IsSteamUser = modalData.SteamProfile != "Non Steam";
                    }

                    var addedRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Добавлен'))");
                    if (addedRow != null)
                    {
                        var addedCell = addedRow.QuerySelector(".span6");
                        modalData.AddedDate = addedCell.GetTextContent();
                    }

                    var expiresRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Истекает'))");
                    if (expiresRow != null)
                    {
                        var expiresCell = expiresRow.QuerySelector(".span6");
                        modalData.ExpiresDate = expiresCell.GetTextContent();
                    }

                    var violationsRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Предыдущих нарушений'))");
                    if (violationsRow != null)
                    {
                        var violationsCell = violationsRow.QuerySelector(".span6");
                        if (int.TryParse(violationsCell.GetTextContent(), out int violations))
                            modalData.PreviousViolations = violations;
                    }

                    var serverRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Забанен на сервере'))");
                    if (serverRow != null)
                    {
                        var serverCell = serverRow.QuerySelector(".span6");
                        modalData.Server = serverCell.GetTextContent();
                    }

                    if (modalData != null)
                    {
                        ban.SteamId = modalData.SteamId;
                        ban.AddedDate = modalData.AddedDate;
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

        private static string ExtractCountry(IElement playerCell)
        {
            var flagImg = playerCell.QuerySelector("img");

            if (flagImg != null)
            {
                var alt = flagImg.GetAttribute("alt");
                return alt ?? "Unknown";
            }

            return "Unknown";
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