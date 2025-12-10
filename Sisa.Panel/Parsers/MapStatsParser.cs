using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;

namespace Sisa.Panel.Parsers
{
    internal class MapStatsParser(IBrowsingContext context) : IParser<IReadOnlyList<MapEntry>>
    {
        public async Task<IReadOnlyList<MapEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var boxes = document.QuerySelectorAll("div.box-content");
            var mapInfos = new List<MapEntry>(boxes.Length);

            foreach (var box in boxes)
            {
                var mapInfo = new MapEntry();
                var nameElement = box.QuerySelector("span.charts-label1");

                if (nameElement != null)
                    mapInfo.Name = nameElement.TextContent;

                var table = box.QuerySelector("table.table.table-condensed.table-hover.table-responsive");
                if (table == null)
                    continue;

                foreach (var row in table.GetTableRows())
                {
                    var cells = row.GetTableCells();

                    var header = cells[0].TextContent;
                    var valueText = cells[1].TextContent;
                    _ = int.TryParse(valueText, out int value);

                    if (header.EqualsOrdinal("Игр"))
                        mapInfo.Games = value;
                    else if (header.EqualsOrdinal("Победы людей"))
                        mapInfo.HumanWins = value;
                    else if (header.EqualsOrdinal("Победы зомби"))
                        mapInfo.ZombieWins = value;
                    else if (header.EqualsOrdinal("Ничьи"))
                        mapInfo.Draws = value;

                }

                if (mapInfo != null && !string.IsNullOrEmpty(mapInfo.Name))
                    mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
    }
}
