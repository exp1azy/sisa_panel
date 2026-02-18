using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;
using System.Globalization;

namespace Sisa.Panel.Parsers
{
    internal partial class PlayerInfoParser(IBrowsingContext context) : IParser<PlayerInfo>
    {
        public async Task<PlayerInfo> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new PlayerInfo
            {
                GeneralInfo = ParseGeneralInfo(document),
                Settings = ParseSettings(document),
                BasicStats = ParseBasicStats(document),
                Progress = ParseProgress(document),
                TempWeapons = ParseTempWeapons(document).ToList().AsReadOnly(),
                WeaponStats = ParseWeaponStats(document).ToList().AsReadOnly(),
                ModWeaponStats = ParseModWeaponStats(document).ToList().AsReadOnly(),
                ZombieClassesStats = ParseZombieClassesStats(document).ToList().AsReadOnly(),
                ZombieGrenades = ParseZombieGrenades(document).ToList().AsReadOnly()
            };
        }

        private static PlayerGeneralInfo ParseGeneralInfo(IDocument document)
        {
            var generalInfo = new PlayerGeneralInfo();

            var avatarElement = document.QuerySelector(".span4 center img.img-circle");
            if (avatarElement != null)
            {
                var avatarSrc = avatarElement.GetAttribute("src");
                if (!string.IsNullOrEmpty(avatarSrc))
                    generalInfo.Image = avatarSrc;
            }

            var nickValueElement = document.QuerySelector(".span8 .value");

            if (nickValueElement != null)
            {
                generalInfo.Country = nickValueElement.ExtractImgAltAttribute();
                var nickText = nickValueElement.TextContent;

                if (!string.IsNullOrEmpty(nickText))
                {
                    var nickAndTag = ParserRegex.TrimUntilNbspPattern.Replace(nickText, "").Split('|');
                    if (nickAndTag.Length == 2)
                    {
                        generalInfo.Tag = nickAndTag[0].Trim();
                        generalInfo.Name = nickAndTag[1].Trim();
                    }
                    else
                    {
                        generalInfo.Name = nickAndTag[0].Trim();
                    }
                }
            }

            var steamLink = document.QuerySelector("a[href*='steamcommunity.com/profiles']");
            if (steamLink != null)
            {
                generalInfo.SteamProfileUrl = steamLink.GetAttribute("href");
                generalInfo.SteamProfileName = steamLink.TextContent.Trim();
                var steamIdMatch = ParserRegex.SteamIdPattern.Match(generalInfo.SteamProfileUrl);

                if (steamIdMatch.Success)
                    generalInfo.SteamId = steamIdMatch.Groups[1].Value;
            }

            var table = document.QuerySelector(".table-responsive");

            if (table == null)
                return generalInfo;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length < 2)
                    continue;

                var label = cells[0].TextContent;
                var value = cells[1].TextContent;

                if (label.EqualsOrdinal("STEAM_ID"))
                {
                    if (string.IsNullOrEmpty(generalInfo.SteamId))
                        generalInfo.SteamId = value;
                }
                else if (label.EqualsOrdinal("Заходил"))
                {
                    generalInfo.LastVisitedAt = value.ParseToDateTime();
                }
                else if (label.EqualsOrdinal("Уровень"))
                {
                    var levelElement = row.QuerySelector(".lvlx");
                    if (levelElement != null && int.TryParse(levelElement.TextContent, out int level))
                        generalInfo.Level = level;
                }
                else if (label.EqualsOrdinal("Состоит в клане"))
                {
                    var clanLink = row.QuerySelector("a");
                    if (clanLink != null)
                        generalInfo.ClanMember = clanLink.TextContent.Trim();
                    else
                        generalInfo.ClanMember = value.Trim();
                }
                else if (label.EqualsOrdinal("Онлайн"))
                {
                    generalInfo.Online = value;
                }
            }

            return generalInfo;
        }

        private static PlayerSettings ParseSettings(IDocument document)
        {
            var settings = new PlayerSettings();

            var settingsSection = document.QuerySelector(".span4.smallstat.box .title");
            if (settingsSection == null || !settingsSection.TextContent.EqualsOrdinal("Настройки"))
                return settings;

            var settingsTable = settingsSection.ParentElement?.QuerySelector(".table-responsive table");
            if (settingsTable == null)
                return settings;

            foreach (var row in settingsTable.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length < 2)
                    continue;

                var settingName = cells[0].TextContent;
                var settingValue = cells[1].TextContent.Trim();

                if (settingName.EqualsOrdinal("Запись демо"))
                    settings.RecordingDemo = settingValue.EqualsOrdinal("Вкл.");
                else if (settingName.EqualsOrdinal("Герой"))
                    settings.Hero = settingValue.EqualsOrdinal("Вкл.");
                else if (settingName.EqualsOrdinal("Героиня"))
                    settings.Heroine = settingValue.EqualsOrdinal("Вкл.");
                else if (settingName.EqualsOrdinal("Класс по умолчанию"))
                    settings.DefaultClass = settingValue;
                else if (settingName.EqualsOrdinal("Язык"))
                    settings.Language = settingValue;
                else if (settingName.EqualsOrdinal("Туторы"))
                    settings.Tutorials = settingValue.EqualsOrdinal("Вкл.");
            }

            return settings;
        }

        private static PlayerBasicStats ParseBasicStats(IDocument document)
        {
            var stats = new PlayerBasicStats();

            var statsSection = document.QuerySelector(".smallstat.box.mobileHalf.span6 .title");
            if (statsSection == null || !statsSection.TextContent.EqualsOrdinal("Основная статистика"))
                return stats;

            var statsTable = statsSection.ParentElement?.QuerySelector(".table-responsive table");
            if (statsTable == null)
                return stats;

            foreach (var row in statsTable.GetTableRows())
            {
                var cells = row.GetTableCells();

                if (cells.Length < 2)
                    continue;

                var label = cells[0].TextContent;
                var valueCell = cells.Length > 2 ? cells[2] : cells[1];

                if (label.EqualsOrdinal("Нож"))
                {
                    var knifeImg = row.QuerySelector("img");
                    if (knifeImg != null)
                    {
                        stats.Knife = knifeImg.GetAttribute("title") ?? string.Empty;
                        stats.KnifeImage = knifeImg.ExtractRelativeImageUrl();
                    }  
                }
                else if (label.EqualsOrdinal("EXP"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int exp))
                        stats.Exp = exp;
                }
                else if (label.EqualsOrdinal("Следующий уровень"))
                {
                    var progressText = valueCell.TextContent;
                    if (!string.IsNullOrEmpty(progressText))
                    {
                        var match = ParserRegex.FormattedNumberExtractorPattern.Match(progressText);

                        if (match.Success && int.TryParse(match.Groups[1].Value.Replace(" ", ""), out int nextLevel))
                            stats.NextLevel = nextLevel;

                        var remainingMatch = ParserRegex.RemainingTimePattern.Match(progressText);

                        if (remainingMatch.Success && int.TryParse(remainingMatch.Groups[1].Value.Replace(" ", ""), out int expToNext))
                            stats.ExpToNextLevel = expToNext;
                    }
                }
                else if (label.EqualsOrdinal("Деньги"))
                {
                    var moneyText = valueCell.TextContent;
                    if (int.TryParse(moneyText.Replace("$", "").Replace(" ", ""), out int money))
                        stats.Money = money;
                }
                else if (label.EqualsOrdinal("Аммо"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int ammo))
                        stats.Ammo = ammo;
                }
                else if (label.EqualsOrdinal("Денежные ключи"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int moneyKeys))
                        stats.MoneyKeys = moneyKeys;
                }
                else if (label.EqualsOrdinal("Аммо ключи"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int ammoKeys))
                        stats.AmmoKeys = ammoKeys;
                }
                else if (label.EqualsOrdinal("Нанес урона"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int damage))
                        stats.Damage = damage;
                }
                else if (label.EqualsOrdinal("Был лучшим"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int mvps))
                        stats.MVPs = mvps;
                }
                else if (label.EqualsOrdinal("Ассистов"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int assists))
                        stats.Assists = assists;
                }
                else if (label.EqualsOrdinal("Самоубийств"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int suicides))
                        stats.Suicides = suicides;
                }
                else if (label.EqualsOrdinal("Смертей"))
                {
                    if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int deaths))
                        stats.Deaths = deaths;
                }
                else if (label.EqualsOrdinal("У/С"))
                {
                    var kdText = valueCell.TextContent;
                    if (decimal.TryParse(kdText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kdRatio))
                        stats.KillDeathRatio = kdRatio;
                }
            }

            return stats;
        }

        private static PlayerProgress ParseProgress(IDocument document)
        {
            var progress = new PlayerProgress();

            foreach (var block in document.QuerySelectorAll(".smallstat.box.mobileHalf.span6"))
            {
                var titleElement = block.QuerySelector(".title");
                if (titleElement?.TextContent.EqualsOrdinal("Достижения") == false)
                    continue;

                var table = block.QuerySelector(".table-responsive table");
                if (table == null)
                    continue;

                foreach (var row in table.GetTableRows())
                {
                    var cells = row.GetTableCells();
                    if (cells.Length < 2) continue;

                    var label = cells[0].TextContent.Trim();
                    var valueText = cells[1].TextContent;
                    var cleanValue = valueText.Replace(" ", "");

                    if (label.EqualsOrdinal("Класс зомби"))
                    {
                        progress.Class = valueText;
                    }
                    else if (label.EqualsOrdinal("Нанес урона (за зм)"))
                    {
                        if (int.TryParse(cleanValue, out int damageByZombie))
                            progress.DamageByZombie = damageByZombie;
                    }
                    else if (label.EqualsOrdinal("Заразил"))
                    {
                        if (int.TryParse(cleanValue, out int infects))
                            progress.Infects = infects;
                    }
                    else if (label.EqualsOrdinal("Был заражен"))
                    {
                        if (int.TryParse(cleanValue, out int wasInfected))
                            progress.WasInfected = wasInfected;
                    }
                    else if (label.EqualsOrdinal("Был первым зомби"))
                    {
                        if (int.TryParse(cleanValue, out int wasFirstZm))
                            progress.WasFirstZm = wasFirstZm;
                    }
                    else if (label.EqualsOrdinal("Был немезисом"))
                    {
                        if (int.TryParse(cleanValue, out int wasNemesis))
                            progress.WasNemesis = wasNemesis;
                    }
                    else if (label.EqualsOrdinal("Был выжившим"))
                    {
                        if (int.TryParse(cleanValue, out int wasSurvivor))
                            progress.WasSurvivor = wasSurvivor;
                    }
                    else if (label.EqualsOrdinal("Был героем"))
                    {
                        if (int.TryParse(cleanValue, out int wasHero))
                            progress.WasHero = wasHero;
                    }
                    else if (label.EqualsOrdinal("Был героиней"))
                    {
                        if (int.TryParse(cleanValue, out int wasHeroine))
                            progress.WasHeroine = wasHeroine;
                    }
                    else if (label.EqualsOrdinal("Убил зомби"))
                    {
                        if (int.TryParse(cleanValue, out int zombieKills))
                            progress.ZombieKills = zombieKills;
                    }
                    else if (label.EqualsOrdinal("Убил людей"))
                    {
                        if (int.TryParse(cleanValue, out int humanKills))
                            progress.HumanKills = humanKills;
                    }
                    else if (label.EqualsOrdinal("Убил немезисов"))
                    {
                        if (int.TryParse(cleanValue, out int nemesisKills))
                            progress.NemesisKills = nemesisKills;
                    }
                    else if (label.EqualsOrdinal("Убил выживших"))
                    {
                        if (int.TryParse(cleanValue, out int survivorKills))
                            progress.SurvivorKills = survivorKills;
                    }
                    else if (label.EqualsOrdinal("Убил боссов"))
                    {
                        if (int.TryParse(cleanValue, out int bossKills))
                            progress.BossKills = bossKills;
                    }
                }
            }

            return progress;
        }

        private static List<PlayerTempWeapon> ParseTempWeapons(IDocument document)
        {
            var weaponsContainer = document.QuerySelector("#wpnhour");
            if (weaponsContainer == null)
                return [];

            var cards = weaponsContainer.QuerySelectorAll(".span4[style*='border: 1px solid #ddd']");
            var tempWeapons = new List<PlayerTempWeapon>(cards.Length);

            foreach (var card in cards)
            {
                var weapon = new PlayerTempWeapon();

                var labelElement = card.QuerySelector(".label");
                if (labelElement != null)
                    weapon.Main = labelElement.ClassList.Contains("label-online");

                var weaponImageElement = card.QuerySelector(".span10 center img");
                weapon.WeaponImage = weaponImageElement?.ExtractRelativeImageUrl() ?? string.Empty;

                var textSpan = card.QuerySelector(".row-fluid:last-child .span12");
                if (textSpan != null)
                {
                    var fullText = textSpan.TextContent;
                    fullText = fullText.Replace("&nbsp;", " ").Replace("\u00A0", " ").Trim();

                    var match = ParserRegex.TempWeaponPattern.Match(fullText);
                    if (match.Success)
                    {
                        weapon.Name = match.Groups[1].Value.Trim();
                        weapon.RemainingTime = match.Groups[2].Value.Trim();
                    }
                    else
                    {
                        weapon.Name = fullText;
                        weapon.RemainingTime = "Неизвестно";
                    }
                }

                tempWeapons.Add(weapon);
            }

            return tempWeapons;
        }

        private static List<PlayerWeaponStatEntry> ParseWeaponStats(IDocument document)
        {
            var table = document.QuerySelector("#weapons table");
            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var weaponStats = new List<PlayerWeaponStatEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 11)
                {
                    var weapon = new PlayerWeaponStatEntry
                    {
                        WeaponImage = cells[0].QuerySelector("a img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        Name = cells[1].TextContent,
                        Shots = ParseIntFromSpan(cells[2]),
                        Hits = ParseIntFromSpan(cells[3]),
                        Accuracy = ParseIntFromProgress(cells[4]),
                        ZombieKills = ParseIntFromSpan(cells[5]),
                        ZombieDamage = ParseIntFromSpan(cells[6]),
                        Assists = ParseIntFromSpan(cells[7]),
                        MVPs = ParseIntFromSpan(cells[8]),
                        Levels = ParseIntFromSpan(cells[9]),
                        BossDamage = ParseIntFromSpan(cells[10]),
                        BossKills = cells.Length > 11 ? ParseIntFromSpan(cells[11]) : 0
                    };

                    weaponStats.Add(weapon);
                }
            }

            return weaponStats;
        }

        private static List<PlayerModWeaponStatEntry> ParseModWeaponStats(IDocument document)
        {
            var table = document.QuerySelector("#modweaps table");
            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var modWeaponStats = new List<PlayerModWeaponStatEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 9)
                {
                    var weapon = new PlayerModWeaponStatEntry
                    {
                        ClassImage = cells[0].QuerySelector("a img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        WeaponImage = cells[1].QuerySelector("center img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        Mod = GetModFromImage(cells[0]),
                        Name = cells[2].TextContent,
                        Shots = ParseIntFromSpan(cells[3]),
                        Hits = ParseIntFromSpan(cells[4]),
                        Accuracy = ParseIntFromProgress(cells[5]),
                        ZombieKills = ParseIntFromSpan(cells[6]),
                        ZombieDamage = ParseIntFromSpan(cells[7]),
                        Assists = ParseIntFromSpan(cells[8]),
                        Levels = cells.Length > 9 ? ParseIntFromSpan(cells[9]) : 0
                    };

                    modWeaponStats.Add(weapon);
                }
            }

            return modWeaponStats;
        }

        private static List<PlayerZombieStatEntry> ParseZombieClassesStats(IDocument document)
        {
            var table = document.QuerySelector("#zmstat");
            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var zombieStats = new List<PlayerZombieStatEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 10)
                {
                    var stat = new PlayerZombieStatEntry
                    {
                        ZombieImage = cells[0].QuerySelector("a img")?.ExtractRelativeImageUrl() ?? string.Empty,
                        Class = GetZombieClassFromImage(cells[0]),
                        RatingPosition = ParseIntFromSpan(cells[1]),
                        Infects = ParseIntFromSpan(cells[2]),
                        HumanKills = ParseIntFromSpan(cells[3]),
                        SurvivorKills = ParseIntFromSpan(cells[4]),
                        Damage = ParseIntFromSpan(cells[5]),
                        Deaths = ParseIntFromSpan(cells[6]),
                        Games = ParseIntFromSpan(cells[7]),
                        WasFirstZm = ParseIntFromSpan(cells[8]),
                        Suicides = cells.Length > 9 ? ParseIntFromSpan(cells[9]) : 0
                    };

                    zombieStats.Add(stat);
                }
            }

            return zombieStats;
        }

        private static List<PlayerZombieGrenadesInfo> ParseZombieGrenades(IDocument document)
        {
            var grenadeLink = document.QuerySelector("a[href*='#zmweapons']");
            if (grenadeLink?.TextContent.ContainsOrdinal("Гранаты зомби") != true)
                return [];

            var grenadesContainer = grenadeLink.Closest(".smallstat.box");
            if (grenadesContainer == null)
                return [];

            var blocks = grenadesContainer.QuerySelectorAll(".span6");
            var grenades = new List<PlayerZombieGrenadesInfo>(blocks.Length);

            foreach (var block in blocks)
            {
                var grenade = new PlayerZombieGrenadesInfo
                {
                    GrenadeImage = block.QuerySelector("center img")?.ExtractRelativeImageUrl() ?? string.Empty
                };

                var nameElement = block.QuerySelector(".charts-label1");
                if (nameElement != null)
                    grenade.Name = nameElement.TextContent;

                var valueElements = block.QuerySelectorAll(".value");
                var labelElements = block.QuerySelectorAll("p[style*='font-size: 14px;color: #c7cbd5;']");

                for (int i = 0; i < labelElements.Length; i++)
                {
                    var label = labelElements[i].TextContent;
                    var valueText = i < valueElements.Length ? valueElements[i].TextContent : "0";

                    if (int.TryParse(valueText, out int value))
                    {
                        if (label.EqualsOrdinal("УРОН"))
                            grenade.Damage = value;
                        else if (label.EqualsOrdinal("ЗАРАЖЕНИЙ"))
                            grenade.Infects = value;
                        else if (label.EqualsOrdinal("БРОСКОВ"))
                            grenade.Throws = value;
                        else if (label.EqualsOrdinal("УБИЙСТВ"))
                            grenade.Kills = value;
                    }
                }

                if (grenade != null)
                    grenades.Add(grenade);
            }

            return grenades;
        }

        private static int ParseIntFromSpan(IElement element)
        {
            var span = element?.QuerySelector("span");

            if (span != null && int.TryParse(span.TextContent.Replace(" ", ""), out int result))
                return result;

            return 0;
        }

        private static int ParseIntFromProgress(IElement element)
        {
            var progress = element?.QuerySelector(".taskProgress");

            if (progress != null && int.TryParse(progress.TextContent.Replace("%", ""), out int result))
                return result;

            return 0;
        }

        private static string GetModFromImage(IElement cell)
        {
            var img = cell.QuerySelector("img");
            if (img != null)
            {
                var src = img.GetAttribute("src") ?? "";
                if (src.Contains("hero", StringComparison.OrdinalIgnoreCase)) return "Hero";
                if (src.Contains("heroine", StringComparison.OrdinalIgnoreCase)) return "Heroine";
                if (src.Contains("surv", StringComparison.OrdinalIgnoreCase)) return "Survivor";
                if (src.Contains("skill", StringComparison.OrdinalIgnoreCase)) return "Skillmod";
            }
            return "Unknown";
        }

        private static string GetZombieClassFromImage(IElement cell)
        {
            var link = cell.QuerySelector("a");
            var title = link?.GetAttribute("title");
            return title ?? "Неизвестно";
        }
    }
}
