using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.Chatlog;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class ChatLogParser(IBrowsingContext context) : BaseParser<ChatLog>(context)
    {
        public override async Task<ChatLog> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var chatLog = new ChatLog
            {
                Messages = ParseMessages(document)
            };

            return chatLog;
        }

        private static List<ChatLogEntry> ParseMessages(IDocument document)
        {
            var messages = new List<ChatLogEntry>();

            var table = document.QuerySelector("table.table-striped");

            if (table == null)
                return messages;

            var rows = table.QuerySelectorAll("tbody tr");

            foreach (var row in rows)
            {
                var columns = row.QuerySelectorAll("td").ToArray();

                if (columns.Length < 6)
                    continue;

                var entry = new ChatLogEntry
                {
                    Time = columns[0].TextContent.Trim(),
                    SteamId = columns[1].TextContent.Trim(),
                    Class = columns[2].QuerySelector("span")?.TextContent.Trim() ?? "Unknown",
                };

                var levelElement = columns[3].QuerySelector("span");
                if (levelElement != null)
                {
                    var levelText = levelElement.TextContent.Trim();

                    if (int.TryParse(levelText, out int level))                    
                        entry.Level = level;                    
                }

                var playerColumn = columns[4];

                var flagImg = playerColumn.QuerySelector("img");
                if (flagImg != null)
                {
                    entry.Country = flagImg.GetAttribute("alt")?.Trim() ?? "Unknown";
                }

                var playerLink = playerColumn.QuerySelector("a");
                entry.PlayerName = playerLink?.TextContent.Trim() ?? "Unknown";

                var messageElement = columns[5].QuerySelector("span.chatmessage");
                entry.Message = messageElement?.TextContent.Trim() ?? columns[5].TextContent.Trim();

                messages.Add(entry);
            }

            return messages;
        }
    }
}