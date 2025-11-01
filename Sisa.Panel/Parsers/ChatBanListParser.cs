using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.ChatBanList;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class ChatBanListParser(IBrowsingContext context) : IParser<ChatBanList>
    {
        public async Task<ChatBanList> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var chatBanList = new ChatBanList()
            {
                ChatBans = ParseChatBansTable(document)
            };

            foreach (var element in document.QuerySelectorAll("td[colspan='8']"))
            {
                var text = element.TextContent;
                if (text.Contains("Всего банов:"))
                {
                    var match = ParserRegex.TotalBansPattern().Match(text);

                    if (match.Success)
                    {
                        if (int.TryParse(match.Groups[1].Value, out int total))
                            chatBanList.TotalBans = total;

                        if (int.TryParse(match.Groups[2].Value, out int active))
                            chatBanList.ActiveBans = active;
                    }
                }
            }

            return chatBanList;
        }

        private static List<ChatBanEntry> ParseChatBansTable(IDocument document)
        {
            var bans = new List<ChatBanEntry>();

            foreach (var row in document.QuerySelectorAll("tr[data-toggle]"))
            {
                var banEntry = new ChatBanEntry();
                var dataTarget = row.GetAttribute("data-target");

                if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWith('#'))
                    banEntry.Id = dataTarget[1..];

                var cells = row.QuerySelectorAll("td");
                if (cells.Length >= 6)
                {
                    banEntry.AddedDate = cells[1].GetTextContent();
                    banEntry.PlayerName = cells[2].GetTextContent();
                    banEntry.Country = ParseCountry(cells[2]);
                    banEntry.AdminName = cells[3].GetTextContent();
                    banEntry.BanType = cells[4].GetTextContent();
                    banEntry.Duration = cells[5].GetTextContent();
                }

                ParseHiddenDetails(row, banEntry);

                if (banEntry != null)
                    bans.Add(banEntry);
            }

            return bans;
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

            foreach (var detailRow in detailsTable.QuerySelectorAll("tr"))
            {
                var cells = detailRow.GetTableCells();
                if (cells.Length >= 2)
                {
                    var key = cells[0].GetTextContent();
                    var value = cells[1].GetTextContent();

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
    }
}
