﻿using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Contest;
using System.Net;

namespace Sisa.Panel.Parsers
{
    internal partial class ContestHistoryParser(IBrowsingContext context) : IParser<IReadOnlyList<ContestHistoryEntry>>
    {
        public async Task<IReadOnlyList<ContestHistoryEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var historyEntries = new List<ContestHistoryEntry>();

            var table = document.QuerySelector("table.table.table-bordered.table-condensed.table-hover.table-responsive");
            if (table == null)
                return historyEntries.AsReadOnly();

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                var entry = new ContestHistoryEntry();

                var numberText = cells[0].GetTextContent();
                _ = int.TryParse(numberText, out int number);
                entry.Number = number;

                var dateText = cells[1].GetTextContent();
                entry.EndsAt = dateText.ParseToDateTime();

                var link = cells[2].QuerySelector("a");
                var name = link.GetTextContent();
                name = WebUtility.HtmlDecode(name);
                name = RemoveSpacesRegex().Replace(name, " ").Trim();
                entry.Winner = name;

                var giftText = cells[3].GetTextContent();
                giftText = WebUtility.HtmlDecode(giftText);
                giftText = RemoveSpacesRegex().Replace(giftText, " ").Trim();
                entry.Gift = giftText;

                if (entry != null)
                    historyEntries.Add(entry);
            }

            return historyEntries.AsReadOnly();
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
        private static partial System.Text.RegularExpressions.Regex RemoveSpacesRegex();
    }
}
