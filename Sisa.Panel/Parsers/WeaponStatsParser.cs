using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class WeaponStatsParser(IBrowsingContext context) : BaseParser<WeaponStats>(context)
    {
        public override async Task<WeaponStats> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var tables = document.QuerySelectorAll("table.table-bordered");
            if (tables.Length == 0) return new();

            var weaponStats = new WeaponStats
            {
                Weapons = ParseWeaponsTable(tables[0]),
                ModWeapons = ParseModWeaponsTable(tables[1])
            };

            return weaponStats;
        }

        private static List<WeaponEntry> ParseWeaponsTable(IElement table)
        {
            var weapons = new List<WeaponEntry>();

            var rows = table.QuerySelectorAll("tbody tr");
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td");
                if (cells.Length >= 13)
                {
                    var weapon = new WeaponEntry
                    {
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
            }

            return weapons;
        }

        private static List<ModWeaponEntry> ParseModWeaponsTable(IElement table)
        {
            var modWeapons = new List<ModWeaponEntry>();

            var rows = table.QuerySelectorAll("tbody tr");
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td");
                if (cells.Length >= 10)
                {
                    var belongsTo = DetermineBelongsTo(cells[0]);

                    var modWeapon = new ModWeaponEntry
                    {
                        BelongsTo = belongsTo,
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
            return span?.TextContent.Trim() ?? element.TextContent.Trim();
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
