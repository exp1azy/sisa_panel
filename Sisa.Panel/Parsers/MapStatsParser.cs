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

            var mapBoxes = document.QuerySelectorAll("div.box.span4");
            var mapInfos = new List<MapEntry>(mapBoxes.Length);

            foreach (var box in mapBoxes)
            {
                var mapInfo = new MapEntry();

                var imageElement = box.QuerySelector(".span6 center img");
                mapInfo.MapImage = imageElement?.ExtractRelativeImageUrl() ?? string.Empty;

                var table = box.QuerySelector("table.table.table-condensed.table-hover.table-responsive");
                if (table == null)
                    continue;

                foreach (var row in table.GetTableRows())
                {
                    var cells = row.GetTableCells();
                    if (cells.Length < 2)
                        continue;

                    var header = cells[0].TextContent.Trim();
                    var valueText = cells[1].TextContent.Trim();

                    if (header.EqualsOrdinal("Карта"))
                    {
                        mapInfo.Name = valueText;
                    }
                    else if (header.EqualsOrdinal("Игр"))
                    {
                        _ = int.TryParse(valueText, out int value);
                        mapInfo.Games = value;
                    }
                    else if (header.EqualsOrdinal("Победы людей"))
                    {
                        _ = int.TryParse(valueText, out int value);
                        mapInfo.HumanWins = value;
                    }
                    else if (header.EqualsOrdinal("Победы зомби"))
                    {
                        _ = int.TryParse(valueText, out int value);
                        mapInfo.ZombieWins = value;
                    }
                    else if (header.EqualsOrdinal("Ничьи"))
                    {
                        _ = int.TryParse(valueText, out int value);
                        mapInfo.Draws = value;
                    }
                }

                if (!string.IsNullOrEmpty(mapInfo.Name))
                    mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
    }
}
