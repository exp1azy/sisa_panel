using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Chatlog;
using Sisa.Panel.Parsers.Interfaces;

namespace Sisa.Panel.Parsers
{
    internal class ChatLogParser(IBrowsingContext context) : IParser<IReadOnlyList<ChatLogEntry>>
    {
        public async Task<IReadOnlyList<ChatLogEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var messages = new List<ChatLogEntry>();

            var table = document.QuerySelector("table.table-striped");
            if (table == null)
                return messages;

            foreach (var row in table.GetTableRows())
            {
                var columns = row.GetTableCells();

                if (columns.Length < 6)
                    continue;

                var entry = new ChatLogEntry
                {
                    Time = columns[0].GetTextContent(),
                    SteamId = columns[1].GetTextContent(),
                    Class = columns[2].QuerySelector("span")?.GetTextContent() ?? "Unknown",
                };

                var levelElement = columns[3].QuerySelector("span");
                if (levelElement != null)
                {
                    var levelText = levelElement.GetTextContent();

                    if (int.TryParse(levelText, out int level))
                        entry.Level = level;
                }

                var playerColumn = columns[4];

                var flagImg = playerColumn.QuerySelector("img");
                if (flagImg != null)
                    entry.Country = flagImg.GetAttribute("alt")?.Trim() ?? "Unknown";

                var playerLink = playerColumn.QuerySelector("a");
                entry.PlayerName = playerLink?.GetTextContent() ?? "Unknown";

                var messageElement = columns[5].QuerySelector("span.chatmessage");
                entry.Message = messageElement?.GetTextContent() ?? columns[5].GetTextContent();

                messages.Add(entry);
            }

            return messages;
        }
    }
}