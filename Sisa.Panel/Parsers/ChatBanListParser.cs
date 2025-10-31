using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.ChatBanList;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal partial class ChatBanListParser(IBrowsingContext context) : IParsable<ChatBanList>
    {
        public async Task<ChatBanList> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var chatBanList = new ChatBanList()
            {
                ChatBans = ParseChatBansTable(document)
            };

            ParseTotalBansInfo(document, chatBanList);

            return chatBanList;
        }

        private static List<ChatBanEntry> ParseChatBansTable(IDocument document)
        {
            var bans = new List<ChatBanEntry>();
            var banRows = document.QuerySelectorAll("tr[data-toggle]");

            foreach (var row in banRows)
            {
                var banEntry = ParseChatBanRow(row);

                if (banEntry != null)
                    bans.Add(banEntry);
            }

            return bans;
        }

        private static ChatBanEntry ParseChatBanRow(IElement row)
        {
            var entry = new ChatBanEntry();
            var dataTarget = row.GetAttribute("data-target");

            if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWith('#'))
                entry.Id = dataTarget[1..];

            var cells = row.QuerySelectorAll("td");
            if (cells.Length >= 6)
            {
                entry.AddedDate = cells[1].TextContent.Trim();
                entry.PlayerName = cells[2].TextContent.Trim();
                entry.Country = ParseCountry(cells[2]);
                entry.AdminName = cells[3].TextContent.Trim();
                entry.BanType = cells[4].TextContent.Trim();
                entry.Duration = cells[5].TextContent.Trim();
            }

            ParseHiddenDetails(row, entry);

            return entry;
        }

        private static string ParseCountry(IElement cell)
        {
            var flagImg = cell.QuerySelector("img");
            if (flagImg != null)
            {
                var altText = flagImg.GetAttribute("alt");

                if (!string.IsNullOrEmpty(altText) && altText != "United States")
                    return altText;
            }

            return "Unknown";
        }

        private static void ParseHiddenDetails(IElement row, ChatBanEntry entry)
        {
            var hiddenRow = row.NextElementSibling;
            if (hiddenRow == null) return;

            var detailsTable = hiddenRow.QuerySelector("table.table-condensed");
            if (detailsTable == null) return;

            var rows = detailsTable.QuerySelectorAll("tr");

            foreach (var detailRow in rows)
            {
                var cells = detailRow.QuerySelectorAll("td");
                if (cells.Length >= 2)
                {
                    var key = cells[0].TextContent.Trim();
                    var value = cells[1].TextContent.Trim();

                    switch (key)
                    {
                        case "Игрок":
                            if (string.IsNullOrEmpty(entry.PlayerName))
                                entry.PlayerName = value;
                            break;
                        case "ID Номер":
                            entry.SteamId = value;
                            break;
                        case "Добавлен":
                            entry.AddedDate = value;
                            break;
                        case "Время бана":
                            entry.BanTime = value;
                            break;
                        case "Истекает":
                            entry.Expires = value;
                            break;
                        case "Предыдущих нарушений":
                            if (int.TryParse(value, out int violations))
                                entry.PreviousViolations = violations;
                            break;
                    }
                }
            }
        }

        private static void ParseTotalBansInfo(IDocument document, ChatBanList chatBanList)
        {
            var totalBansElements = document.QuerySelectorAll("td[colspan='8']");

            foreach (var element in totalBansElements)
            {
                var text = element.TextContent;
                if (text.Contains("Всего банов:"))
                {
                    var match = TotalBansRegex().Match(text);

                    if (match.Success)
                    {
                        if (int.TryParse(match.Groups[1].Value, out int total))
                            chatBanList.TotalBans = total;

                        if (int.TryParse(match.Groups[2].Value, out int active))
                            chatBanList.ActiveBans = active;
                    }
                }
            }
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"Всего банов:\s*(\d+)\s*\((\d+)\s*active\)")]
        private static partial System.Text.RegularExpressions.Regex TotalBansRegex();
    }
}
