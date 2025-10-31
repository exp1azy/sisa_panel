using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class HumanTopPlayersParser(IBrowsingContext context) : IParser<HumanTopPlayersStat>
    {
        public async Task<HumanTopPlayersStat> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new HumanTopPlayersStat
            {
                Human = ParseHumanClass(document),
                Survivor = ParseSurvivorClass(document),
                Heroine = ParseHeroineClass(document),
                AdvancedMod = ParseAdvancedModClass(document),
                WomanMod = ParseWomanModClass(document),
                Hero = ParseHeroClass(document)
            };
        }

        private static HumanClassInfo ParseHumanClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Human']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Человек",
                Health = ParsePropertyValue(section, "Здоровье", 0),
                Gravity = ParsePropertyValue(section, "Гравитация", 1),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                WasInfected = ParseStatValue(section, "Был заражен"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_human")
            };
        }

        private static HumanClassInfo ParseSurvivorClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Survivor']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Выживший",
                Speed = ParsePropertyValue(section, "Скорость", 0),
                Gravity = ParsePropertyValue(section, "Гравитация", 1),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                Games = ParseStatValue(section, "Игр"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_survivor")
            };
        }

        private static HumanClassInfo ParseHeroineClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Heroine']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Героиня",
                Health = ParsePropertyValue(section, "Здоровье", 0),
                Speed = ParsePropertyValue(section, "Скорость", 1),
                Gravity = ParsePropertyValue(section, "Гравитация", 2),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                Games = ParseStatValue(section, "Игр"),
                WasInfected = ParseStatValue(section, "Был заражен"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_heroine")
            };
        }

        private static HumanClassInfo ParseAdvancedModClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Skillmod']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Продвинутый мод",
                Health = ParsePropertyValue(section, "Здоровье", 0),
                Gravity = ParsePropertyValue(section, "Гравитация", 1),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                WasInfected = ParseStatValue(section, "Был заражен"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_skillmod")
            };
        }

        private static HumanClassInfo ParseWomanModClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Girlmod']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Женский мод",
                Health = ParsePropertyValue(section, "Здоровье", 0),
                Gravity = ParsePropertyValue(section, "Гравитация", 1),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                WasInfected = ParseStatValue(section, "Был заражен"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_girlmod")
            };
        }

        private static HumanClassInfo ParseHeroClass(IDocument document)
        {
            var section = document.QuerySelector("div[id='Hero']");
            if (section == null) return null;

            return new HumanClassInfo
            {
                Name = "Герой",
                Health = ParsePropertyValue(section, "Здоровье", 0),
                Speed = ParsePropertyValue(section, "Скорость", 1),              
                Gravity = ParsePropertyValue(section, "Гравитация", 2),
                ZombieKills = ParseStatValue(section, "Убийств Зомби"),
                NemesisKills = ParseStatValue(section, "Убийств Немезиса"),
                Deaths = ParseStatValue(section, "Смертей"),
                Games = ParseStatValue(section, "Игр"),
                WasInfected = ParseStatValue(section, "Был заражен"),
                Suicides = ParseStatValue(section, "Самоубийств"),
                Damage = ParseStatValueLong(section, "Урон"),
                Players = ParsePlayers(document, "hm_hero")
            };
        }

        private static HumanTopPlayerEntry[] ParsePlayers(IDocument document, string sectionId)
        {
            var players = new List<HumanTopPlayerEntry>();
            var section = document.QuerySelector($"div[id='{sectionId}']");

            if (section != null)
            {
                var table = section.QuerySelector("table.sortable");
                if (table != null)
                {
                    foreach (var row in table.GetTableRows())
                    {
                        var cells = row.GetTableCells();
                        if (cells.Length < 6) return null;

                        var player = new HumanTopPlayerEntry
                        {
                            RatingPosition = ParseCellValueInt(row, 0),
                            Name = ParsePlayerName(cells[1]),
                            Country = ParseCountry(cells[1]),
                            ZombieKills = ParseCellValueInt(row, 2),
                            Damage = ParseCellValueLong(row, 3),
                            Deaths = ParseCellValueInt(row, 5),
                            Games = ParseCellValueInt(row, 6),
                            WasInfected = ParseCellValueInt(row, 4)
                        };

                        player.Games = player.Games == 0 ? null : player.Games;
                        player.WasInfected = player.WasInfected == 0 ? null : player.WasInfected;

                        players.Add(player);
                    }
                }
            }

            return players.ToArray();
        }

        private static long ParseCellValueLong(IElement row, int cellIndex)
        {
            var cells = row.GetTableCells();
            if (cells.Length > cellIndex)
            {
                var text = cells[cellIndex].GetTextContent();

                if (long.TryParse(text, out long value))
                    return value;
            }

            return default;
        }

        private static int ParseCellValueInt(IElement row, int cellIndex)
        {
            var cells = row.GetTableCells();
            if (cells.Length > cellIndex)
            {
                var text = cells[cellIndex].GetTextContent();

                if (int.TryParse(text, out int value))
                    return value;
            }

            return default;
        }

        private static int ParsePropertyValue(IElement section, string propertyName, int index)
        {
            var propertyRow = section
                .QuerySelectorAll("table.table tbody tr")
                .FirstOrDefault(tr => tr.TextContent.Contains(propertyName));

            if (propertyRow != null)
            {
                var valueCell = propertyRow.GetTableCells()[index].TextContent.Split(' ')[1];

                if (valueCell != null && int.TryParse(valueCell.Trim(), out int value))
                    return value;
            }

            return default;
        }

        private static int ParseStatValue(IElement section, string statName)
        {
            var statRow = section
                .QuerySelectorAll("table.table-condensed tbody tr")
                .FirstOrDefault(tr => tr.TextContent.Contains(statName));

            if (statRow != null)
            {
                var valueCell = statRow.QuerySelector("td:last-child");

                if (valueCell != null && int.TryParse(valueCell.GetTextContent(), out int value))
                    return value;
            }

            return default;
        }

        private static long ParseStatValueLong(IElement section, string statName)
        {
            var statRow = section.QuerySelectorAll("table.table-condensed tbody tr")
                .FirstOrDefault(tr => tr.TextContent.Contains(statName));

            if (statRow != null)
            {
                var valueCell = statRow.QuerySelector("td:last-child");

                if (valueCell != null && long.TryParse(valueCell.GetTextContent(), out long value))
                    return value;
            }

            return default;
        }

        private static string ParsePlayerName(IElement cell)
        {
            var link = cell.QuerySelector("a");
            return link?.GetTextContent() ?? cell.GetTextContent();
        }

        private static string ParseCountry(IElement element)
        {
            var img = element.QuerySelector("img");
            var alt = img?.GetAttribute("alt");
            return alt ?? "Unknown";
        }
    }
}
