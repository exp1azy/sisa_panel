using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Contest;
using Sisa.Panel.Parsers.Interfaces;

namespace Sisa.Panel.Parsers
{
    internal class ContestParticipantsParser(IBrowsingContext context) : IParser<IReadOnlyList<ContestParticipant>>
    {
        public async Task<IReadOnlyList<ContestParticipant>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var participants = new List<ContestParticipant>();

            var table = document.QuerySelector("table.table.table-bordered.table-condensed.table-hover.table-responsive");
            if (table == null)
                return participants.AsReadOnly();

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                var participant = new ContestParticipant();

                var flagImg = cells[1].QuerySelector("img");
                participant.Country = flagImg?.GetAttribute("alt") ?? "Unknown";

                var link = cells[1].QuerySelector("a");
                var name = link.GetTextContent();
                name = ParserRegex.WhitespaceCleanupPattern().Replace(name, " ").Trim();
                participant.Name = name;

                var dateText = cells[2].GetTextContent();
                participant.RegisteredAt = dateText.ParseToDateTime();

                if (participant != null)
                    participants.Add(participant);
            }

            return participants.AsReadOnly();
        }
    }
}
