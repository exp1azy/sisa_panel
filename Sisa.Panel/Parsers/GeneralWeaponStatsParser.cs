using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class GeneralWeaponStatsParser(IBrowsingContext context) : IParser<WeaponStats>
    {
        public async Task<WeaponStats> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var tables = document.QuerySelectorAll("table.table-bordered");
            if (tables.Length == 0) return new();

            return new WeaponStats
            {
                Weapons = ParseWeaponsTable(tables[0]),
                ModWeapons = ParseModWeaponsTable(tables[1])
            };
        }

        private static List<WeaponEntry> ParseWeaponsTable(IElement table)
        {
            var rows = table.GetTableRows();
            var weapons = new List<WeaponEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();

                if (cells.Length < 13)
                    continue;

                var weapon = new WeaponEntry
                {
                    WeaponImage = cells[0].QuerySelector("img")?.ExtractRelativeImageUrl() ?? string.Empty,
                    Wid = ParseWid(cells[1]),
                    Name = GetTextContent(cells[1]),
                    Shots = ParseInt(GetTextContent(cells[2])),
                    ZombieKills = ParseInt(GetTextContent(cells[3])),
                    ZombieDamage = ParseLong(GetTextContent(cells[4])),
                    Hits = ParseInt(GetTextContent(cells[5])),
                    Assists = ParseInt(GetTextContent(cells[6])),
                    MVPs = ParseInt(GetTextContent(cells[7])),
                    Levels = ParseInt(GetTextContent(cells[8])),
                    BossDamage = ParseInt(GetTextContent(cells[9])),
                    BossKills = ParseInt(GetTextContent(cells[10])),
                    AvailableFromLevel = ParseInt(GetTextContent(cells[11])),
                    Cost = ParseInt(GetTextContent(cells[12])),
                    Ratio = ParseInt(GetTextContent(cells[13]))
                };

                weapons.Add(weapon);
            }

            return weapons;
        }

        private static int ParseWid(IElement cell)
        {
            var link = cell.QuerySelector("a[href*='wid=']");
            if (link != null)
            {
                var href = link.GetAttribute("href") ?? "";
                var widMatch = ParserRegex.WidPattern.Match(href);

                if (widMatch.Success && int.TryParse(widMatch.Groups[1].Value, out int wid))
                    return wid;
            }

            return 0;
        }

        private static List<ModWeaponEntry> ParseModWeaponsTable(IElement table)
        {
            var rows = table.GetTableRows();
            var modWeapons = new List<ModWeaponEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 10)
                {
                    var belongsTo = DetermineBelongsTo(cells[0]);

                    var modWeapon = new ModWeaponEntry
                    {
                        BelongsTo = belongsTo,
                        ClassImage = cells[0].QuerySelector("img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        WeaponImage = cells[1].QuerySelector("img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        Name = GetTextContent(cells[2]),
                        Shots = ParseInt(GetTextContent(cells[3])),
                        ZombieKills = ParseInt(GetTextContent(cells[4])),
                        ZombieDamage = ParseLong(GetTextContent(cells[5])),
                        Hits = ParseInt(GetTextContent(cells[6])),
                        Assists = ParseInt(GetTextContent(cells[7])),
                        MVPs = ParseInt(GetTextContent(cells[8])),
                        Levels = ParseInt(GetTextContent(cells[9])),
                        Ratio = ParseInt(GetTextContent(cells[10]))
                    };

                    modWeapons.Add(modWeapon);
                }
            }

            return modWeapons;
        }

        private static string DetermineBelongsTo(IElement cell)
        {
            var img = cell.QuerySelector("img");
            var src = img.GetAttribute("title");
            return src ?? "Unknown";
        }

        private static string GetTextContent(IElement element)
        {
            var span = element.QuerySelector("span");
            return span?.TextContent ?? element.TextContent;
        }

        private static int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-") return 0;
            return int.TryParse(value.Replace(" ", ""), out int result) ? result : 0;
        }

        private static long ParseLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-") return 0;
            return long.TryParse(value.Replace(" ", ""), out long result) ? result : 0;
        }
    }
}
