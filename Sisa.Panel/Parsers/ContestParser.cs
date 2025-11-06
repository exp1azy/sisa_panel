using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Contest;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class ContestParser(IBrowsingContext context) : IParser<ContestInfo>
    {
        public async Task<ContestInfo> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new ContestInfo
            {
                LastContestWinners = ParseLastWinners(document),
                CurrentContestParticipants = ParseCurrentParticipants(document)
            };
        }

        private static List<LastWinner> ParseLastWinners(IDocument document)
        {
            var winners = new List<LastWinner>();

            var prevContestSection = document
                .QuerySelectorAll("center")
                .FirstOrDefault(x => x.TextContent.Contains("Предыдущий конкурс"))?.ParentElement;

            if (prevContestSection != null)
            {
                foreach (var block in prevContestSection.QuerySelectorAll(".span4"))
                {
                    var span = block.QuerySelector("span.charts-label1");
                    var nameLink = span.GetSteamProfileElement();

                    var name = nameLink?.TextContent;
                    var steamProfile = nameLink?.GetAttribute("href") ?? string.Empty;
                    var para = block.QuerySelector("p.charts-label1")?.TextContent;

                    var winner = new LastWinner
                    {
                        Name = name,
                        SteamProfile = steamProfile,
                        Gift = para
                    };

                    winners.Add(winner);
                }
            }

            return winners;
        }

        private static List<ContestParticipant> ParseCurrentParticipants(IDocument document)
        {
            var participants = new List<ContestParticipant>();

            var table = document.QuerySelector("table.table-bordered");
            if (table == null)
                return participants;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length < 3)
                    continue;

                var flagImg = cells[1].QuerySelector("img");
                var country = flagImg?.GetAttribute("alt") ?? "Неизвестно";

                var nameLink = cells[1].QuerySelector("a");
                var name = nameLink?.GetTextContent();

                if (string.IsNullOrEmpty(name))
                    name = cells[1].GetTextContent();

                var dateText = cells[2].TextContent;
                var registeredAt = dateText.ParseToDateTime();

                var participant = new ContestParticipant
                {
                    Country = country,
                    Name = name ?? "Неизвестно",
                    RegisteredAt = registeredAt
                };

                if (participant != null)
                    participants.Add(participant);
            }

            return participants;
        }
    }
}
