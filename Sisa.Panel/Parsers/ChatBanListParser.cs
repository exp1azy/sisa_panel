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
                if (text.ContainsOrdinal("Всего банов:"))
                {
                    var match = ParserRegex.TotalBansPattern.Match(text);

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
            var rows = document.QuerySelectorAll("tr[data-toggle]");
            var bans = new List<ChatBanEntry>(rows.Length);

            foreach (var row in rows)
            {
                var banEntry = new ChatBanEntry();
                var dataTarget = row.GetAttribute("data-target");

                if (!string.IsNullOrEmpty(dataTarget) && dataTarget.StartsWithOrdinal("#"))
                    banEntry.Id = dataTarget[1..];

                var cells = row.GetTableCells();
                if (cells.Length >= 6)
                {
                    banEntry.Date = cells[1].TextContent;
                    banEntry.PlayerName = cells[2].TextContent.Trim();
                    banEntry.Country = cells[2].ExtractImgAltAttribute();
                    banEntry.AdminName = cells[3].TextContent;
                    banEntry.BanType = cells[4].TextContent;
                    banEntry.Duration = cells[5].TextContent;
                }

                ParseHiddenDetails(row, banEntry);

                if (banEntry != null)
                    bans.Add(banEntry);
            }

            return bans;
        }

        private static void ParseHiddenDetails(IElement row, ChatBanEntry entry)
        {
            var hiddenRow = row.NextElementSibling;
            if (hiddenRow == null) return;

            var detailsTable = hiddenRow.QuerySelector("table.table-condensed");
            if (detailsTable == null) return;

            foreach (var detailRow in detailsTable.GetTableRows())
            {
                var cells = detailRow.GetTableCells();

                if (cells.Length < 2)
                    continue;

                var key = cells[0].TextContent;
                var value = cells[1];

                if (key.EqualsOrdinal("Игрок"))
                {
                    if (string.IsNullOrEmpty(entry.PlayerName))
                        entry.PlayerName = value.TextContent;
                }
                else if (key.EqualsOrdinal("ID Номер"))
                {
                    entry.SteamId = value.TextContent;
                }
                else if (key.EqualsOrdinal("Steam профиль"))
                {
                    var link = value.GetSteamProfileElement();
                    entry.SteamProfile = link?.GetAttribute("href") ?? string.Empty;
                }
                else if (key.EqualsOrdinal("Добавлен"))
                {
                    entry.Date = value.TextContent;
                }
                else if (key.EqualsOrdinal("Время бана"))
                {
                    entry.BanTime = value.TextContent;
                }
                else if (key.EqualsOrdinal("Истекает"))
                {
                    entry.Expires = value.TextContent;
                }
                else if (key.EqualsOrdinal("Предыдущих нарушений"))
                {
                    if (int.TryParse(value.TextContent, out int violations))
                        entry.PreviousViolations = violations;
                }
            }
        }
    }
}
