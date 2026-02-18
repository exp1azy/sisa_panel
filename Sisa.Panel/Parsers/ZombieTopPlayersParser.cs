using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class ZombieTopPlayersParser(IBrowsingContext context) : IParser<ZombieTopPlayersStats>
    {
        public async Task<ZombieTopPlayersStats> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new ZombieTopPlayersStats
            {
                Classic = ParseZombieClass(document, "Classic", "zm_classic"),
                Fast = ParseZombieClass(document, "Fast", "zm_fast"),
                Healer = ParseZombieClass(document, "Healer", "zm_healer"),
                Big = ParseZombieClass(document, "Big", "zm_big"),
                Voodo = ParseZombieClass(document, "Voodo", "zm_voodo"),
                Hunter = ParseZombieClass(document, "Hunter", "zm_hunter"),
                Tesla = ParseZombieClass(document, "Tesla", "zm_tesla"),
                Nemesis = ParseZombieClass(document, "Nemesis", "zm_nemesis")
            };
        }

        private static ZombieClassInfo ParseZombieClass(IDocument document, string className, string playersSectionId)
        {
            var classInfo = new ZombieClassInfo
            {
                Name = className
            };

            var section = document.QuerySelector($"div[id='{className}']");
            if (section == null) return classInfo;

            classInfo.ClassImage = section.QuerySelector(".span4 img")?.ExtractRelativeImageUrl() ?? string.Empty;

            if (!className.EqualsOrdinal("Немезис"))
            {
                var propertiesTable = section.QuerySelector("table.table:first-of-type");
                if (propertiesTable == null) return classInfo;

                foreach (var row in propertiesTable.GetTableRows())
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        if (cell.TextContent.ContainsOrdinal("Здоровье:"))
                            classInfo.Health = ParsePropertyValue(cell.TextContent, "Здоровье:");
                        else if (cell.TextContent.ContainsOrdinal("Скорость:"))
                            classInfo.Speed = ParsePropertyValue(cell.TextContent, "Скорость:");
                        else if (cell.TextContent.ContainsOrdinal("Отброс:"))
                            classInfo.Knockback = cell.TextContent.Replace("Отброс:", string.Empty).Trim();
                    }
                }
            }

            var statsTable = section.QuerySelector("table.table-condensed:not(.sortable)");
            if (statsTable == null) return classInfo;

            foreach (var row in statsTable.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length < 2)
                    continue;

                var statName = cells[0].TextContent;
                var statValueText = cells[1].TextContent;

                if (statName.EqualsOrdinal("Заражений"))
                    classInfo.Infects = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Убийств людей"))
                    classInfo.HumansKills = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Убийств выживших"))
                    classInfo.SurvivorsKills = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Смертей"))
                    classInfo.Deaths = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Игр"))
                    classInfo.Games = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Урон"))
                    classInfo.Damage = ParseLongValue(statValueText);
                else if (statName.EqualsOrdinal("Первый зомби"))
                    classInfo.FirstZm = ParseIntValue(statValueText);
                else if (statName.EqualsOrdinal("Самоубийств"))
                    classInfo.Suicides = ParseIntValue(statValueText);               
            }

            var playersSection = document.QuerySelector($"div[id='{playersSectionId}']");

            if (playersSection != null)
                classInfo.Players = ParseZombieTopPlayers(playersSection).ToArray();

            return classInfo;
        }

        private static List<ZombieTopPlayerEntry> ParseZombieTopPlayers(IElement section)
        {
            var playersTable = section.QuerySelector("table.sortable");

            if (playersTable == null) 
                return [];

            var rows = playersTable.GetTableRows();
            var players = new List<ZombieTopPlayerEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length < 7) return players;

                ZombieTopPlayerEntry? player;

                if (cells.Length > 7)
                {
                    player = new ZombieTopPlayerEntry
                    {
                        RatingPosition = ParseCellValueInt(cells[0]),
                        Infects = ParseCellValueInt(cells[2]),
                        HumanKills = ParseCellValueInt(cells[3]),
                        SurvivorKills = ParseCellValueInt(cells[4]),
                        Damage = ParseCellValueLong(cells[5]),
                        Deaths = ParseCellValueInt(cells[6]),
                        Games = ParseCellValueInt(cells[7])
                    };
                }
                else
                {
                    player = new ZombieTopPlayerEntry
                    {
                        RatingPosition = ParseCellValueInt(cells[0]),
                        HumanKills = ParseCellValueInt(cells[2]),
                        SurvivorKills = ParseCellValueInt(cells[3]),
                        Damage = ParseCellValueLong(cells[4]),
                        Deaths = ParseCellValueInt(cells[5]),
                        Games = ParseCellValueInt(cells[6])
                    };
                }

                player.Name = cells[1].ExtractLinkText().Trim();
                player.Country = cells[1].ExtractImgAltAttribute();

                if (player != null)
                    players.Add(player);
            }

            return players;
        }

        private static int ParsePropertyValue(string text, string propertyName)
        {
            var valueText = text.Replace(propertyName, "").Trim();
            return ParseIntValue(valueText);
        }

        private static int ParseIntValue(string text)
        {
            if (int.TryParse(text, out int value))
                return value;

            return default;
        }

        private static long ParseLongValue(string text)
        {
            if (long.TryParse(text, out long value))
                return value;

            return 0;
        }

        private static int ParseCellValueInt(IElement cell)
        {
            var text = cell.TextContent;
            return ParseIntValue(text);
        }

        private static long ParseCellValueLong(IElement cell)
        {
            var text = cell.TextContent;
            return ParseLongValue(text);
        }
    }
}
