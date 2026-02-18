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
            var table = document.QuerySelector("table.table-striped");

            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var messages = new List<ChatLogEntry>(rows.Length);

            foreach (var row in rows)
            {
                var columns = row.GetTableCells();

                if (columns.Length < 6)
                    continue;

                var entry = new ChatLogEntry
                {
                    Time = columns[0].TextContent,
                    SteamId = columns[1].TextContent.Trim(),
                    Class = columns[2].QuerySelector("span")?.TextContent ?? "Unknown",
                };

                var levelElement = columns[3].QuerySelector("span");
                if (levelElement != null)
                {
                    var levelText = levelElement.TextContent;

                    if (int.TryParse(levelText, out int level))
                        entry.Level = level;
                }

                entry.Country = columns[4].ExtractImgAltAttribute();
                entry.PlayerName = columns[4].ExtractLinkText().Trim();
                entry.Image = columns[4].ExtractAbsoluteImageUrl();

                var messageElement = columns[5].QuerySelector("span.chatmessage");
                entry.Message = messageElement?.TextContent.Trim() ?? columns[5].TextContent.Trim();

                messages.Add(entry);
            }

            return messages;
        }
    }
}