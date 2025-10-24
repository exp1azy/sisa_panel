using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal partial class BanListParser(IBrowsingContext context) : BaseParser<BanList>(context)
    {
        public override async Task<BanList> ParseAsync(string htmlContent)
        {
            var document = await Context.OpenAsync(req => req.Content(htmlContent));

            var banList = new BanList
            {
                Bans = ParseBansTable(document)
            };

            ParseBanListInfo(document, banList);

            return banList;
        }

        private static List<BanEntry> ParseBansTable(IDocument document)
        {
            var bans = new List<BanEntry>();
            var rows = document.QuerySelectorAll("table.table tbody tr");

            foreach (var row in rows)
            {
                if (row.QuerySelector("td[colspan]") != null)
                    continue;

                var ban = ParseBanRow(row);

                if (ban != null)
                {
                    var modalData = ParseModalData(document, ban.BanId);

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

        private static BanEntry ParseBanRow(IElement row)
        {
            var cols = row.QuerySelectorAll("td");

            if (cols.Length < 7)
                return null;

            var banId = ExtractBanIdFromRow(row);
            var banType = DetermineBanType(cols[0]);

            return new BanEntry
            {
                BanId = banId,
                Date = cols[1].TextContent.Trim(),
                PlayerName = ExtractPlayerName(cols[2]),
                Country = ExtractCountry(cols[2]),
                AdminName = cols[3].TextContent.Trim(),
                Reason = cols[4].TextContent.Trim(),
                Duration = cols[5].TextContent.Trim(),
                HasDemo = !cols[6].TextContent.Contains("нет демо"),
                BanType = banType
            };
        }

        private static int ExtractBanIdFromRow(IElement row)
        {
            var dataTarget = row.GetAttribute("data-target");

            if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWith("#ban-"))
            {
                var idStr = dataTarget[5..];

                if (int.TryParse(idStr, out int banId))
                    return banId;
            }

            return 0;
        }

        private static string DetermineBanType(IElement typeCell)
        {
            var content = typeCell.InnerHtml;

            if (content.Contains("internet-explorer"))
                return "website";
            else if (content.Contains("cstrike.gif"))
                return "cs";
            else if (content.Contains("czero.gif"))
                return "czero";

            return "unknown";
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

        private static BanEntry ParseModalData(IDocument document, int banId)
        {
            var modal = document.QuerySelector($"#ban-{banId}");

            if (modal == null) 
                return null;

            var modalData = new BanEntry();

            var steamIdRow = modal.QuerySelector(".row-fluid:has(.span5:contains('ID Номер'))");
            if (steamIdRow != null)
            {
                var steamIdCell = steamIdRow.QuerySelector(".span6");
                modalData.SteamId = steamIdCell?.TextContent.Trim();
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
                modalData.AddedDate = addedCell?.TextContent.Trim();
            }

            var expiresRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Истекает'))");
            if (expiresRow != null)
            {
                var expiresCell = expiresRow.QuerySelector(".span6");
                modalData.ExpiresDate = expiresCell?.TextContent.Trim();
            }

            var violationsRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Предыдущих нарушений'))");
            if (violationsRow != null)
            {
                var violationsCell = violationsRow.QuerySelector(".span6");
                if (int.TryParse(violationsCell?.TextContent.Trim(), out int violations))
                    modalData.PreviousViolations = violations;
            }

            var serverRow = modal.QuerySelector(".row-fluid:has(.span5:contains('Забанен на сервере'))");
            if (serverRow != null)
            {
                var serverCell = serverRow.QuerySelector(".span6");
                modalData.Server = serverCell?.TextContent.Trim();
            }

            return modalData;
        }

        private static void ParseBanListInfo(IDocument document, BanList banList)
        {
            var summaryRow = document.QuerySelector("td[colspan]");
            if (summaryRow != null)
            {
                var summaryText = summaryRow.TextContent;

                var bansMatch = BansMatch().Match(summaryText);
                if (bansMatch.Success)
                {
                    banList.TotalBans = int.Parse(bansMatch.Groups[1].Value);
                    banList.ActiveBans = int.Parse(bansMatch.Groups[2].Value);
                }

                var demosMatch = DemosMatch().Match(summaryText);
                if (demosMatch.Success)
                {
                    banList.TotalDemos = int.Parse(demosMatch.Groups[1].Value);
                }
            }
        }

        private static string ExtractSteamProfile(IElement steamProfileCell)
        {
            if (steamProfileCell == null) 
                return "Unknown";

            var steamIcon = steamProfileCell.QuerySelector("i.fa-steam");
            if (steamIcon != null)
            {
                var steamLink = steamProfileCell.QuerySelector("a[href*='steamcommunity.com']");

                if (steamLink != null)
                    return steamLink.GetAttribute("href");

                return "Steam User (No link)";
            }

            var nonSteamIcon = steamProfileCell.QuerySelector("i.fa-times");

            if (nonSteamIcon != null)
                return "Non Steam";

            return "Unknown";
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"Всего банов:\s*(\d+)\s*\((\d+)\s*Active\)")]
        private static partial System.Text.RegularExpressions.Regex BansMatch();

        [System.Text.RegularExpressions.GeneratedRegex(@"Всего демо в базе данных\s*:\s*(\d+)")]
        private static partial System.Text.RegularExpressions.Regex DemosMatch();
    }
}