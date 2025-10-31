using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Parsers
{
    internal class MapStatsParser(IBrowsingContext context) : IParsable<IReadOnlyList<MapEntry>>
    {
        public async Task<IReadOnlyList<MapEntry>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var mapInfos = new List<MapEntry>();
            var mapBoxes = document.QuerySelectorAll("div.box-content");

            foreach (var box in mapBoxes)
            {
                var mapInfo = ParseMapBox(box);

                if (mapInfo != null && !string.IsNullOrEmpty(mapInfo.Name))
                    mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }

        private static MapEntry ParseMapBox(IElement box)
        {
            var mapInfo = new MapEntry();
            var nameElement = box.QuerySelector("span.charts-label1");

            if (nameElement != null)
                mapInfo.Name = nameElement.TextContent.Trim();

            var table = box.QuerySelector("table");
            if (table != null)
            {
                var rows = table.QuerySelectorAll("tr");
                foreach (var row in rows)
                {
                    var cells = row.QuerySelectorAll("td");
                    if (cells.Length >= 2)
                    {
                        var header = cells[0].TextContent.Trim();
                        var valueText = cells[1].TextContent.Trim();

                        if (int.TryParse(valueText, out int value))
                        {
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
                    }
                }
            }

            return mapInfo;
        }
    }
}
