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
            var mapInfos = new List<MapEntry>();

            foreach (var box in document.QuerySelectorAll("div.box-content"))
            {
                var mapInfo = new MapEntry();
                var nameElement = box.QuerySelector("span.charts-label1");

                if (nameElement != null)
                    mapInfo.Name = nameElement.GetTextContent();

                var table = box.QuerySelector("table.table.table-condensed.table-hover.table-responsive");
                if (table == null)
                    continue;

                foreach (var row in table.QuerySelectorAll("tr"))
                {
                    var cells = row.GetTableCells();

                    var header = cells[0].GetTextContent();
                    var valueText = cells[1].GetTextContent();
                    _ = int.TryParse(valueText, out int value);

                    switch (header)
                    {
                        case "Игр":
                            mapInfo.Games = value;
                            break;
                        case "Победы людей":
                            mapInfo.HumanWins = value;
                            break;
                        case "Победы зомби":
                            mapInfo.ZombieWins = value;
                            break;
                        case "Ничьи":
                            mapInfo.Draws = value;
                            break;
                    }
                }

                if (mapInfo != null && !string.IsNullOrEmpty(mapInfo.Name))
                    mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
    }
}
