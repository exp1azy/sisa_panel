using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Stat;
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

            var nickValueElement = document.QuerySelector(".span8 .value");
            if (nickValueElement != null)
            {
                var flagImg = nickValueElement.QuerySelector("img");
                if (flagImg != null)
                {
                    var altText = flagImg.GetAttribute("alt");

                    if (!string.IsNullOrEmpty(altText))
                        generalInfo.Country = altText;
                }

                var nickText = nickValueElement.GetTextContent();
                if (!string.IsNullOrEmpty(nickText))
                {
                    var nickAndTag = NickTextRegex().Replace(nickText, "").Split('|');
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
                generalInfo.SteamProfileName = steamLink.GetTextContent();
                var steamIdMatch = SteamIdRegex().Match(generalInfo.SteamProfileUrl);

                if (steamIdMatch.Success)
                    generalInfo.SteamId = steamIdMatch.Groups[1].Value;
            }

            var table = document.QuerySelector(".table-responsive");
            if (table == null)
                return generalInfo;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 2)
                {
                    var label = cells[0].GetTextContent();
                    var value = cells[1].GetTextContent();

                    switch (label)
                    {
                        case "STEAM_ID":
                            if (string.IsNullOrEmpty(generalInfo.SteamId))
                                generalInfo.SteamId = value;
                            break;

                        case "Заходил":
                                generalInfo.LastVisitedAt = value.ParseToDateTime();
                            break;

                        case "Уровень":
                            var levelElement = row.QuerySelector(".lvlx");
                            if (levelElement != null && int.TryParse(levelElement.GetTextContent(), out int level))
                                generalInfo.Level = level;
                            break;

                        case "Состоит в клане":
                            var clanLink = row.QuerySelector("a");
                            if (clanLink != null)
                                generalInfo.ClanMember = clanLink.GetTextContent();
                            else
                                generalInfo.ClanMember = value;
                            break;

                        case "Онлайн":
                            generalInfo.Online = value;
                            break;
                    }
                }
            }

            return generalInfo;
        }

        private static PlayerSettings ParseSettings(IDocument document)
        {
            var settings = new PlayerSettings();

            var settingsSection = document.QuerySelector(".span4.smallstat.box .title");
            if (settingsSection == null || settingsSection.TextContent != "Настройки")
                return settings;

            var settingsTable = settingsSection.ParentElement?.QuerySelector(".table-responsive table");
            if (settingsTable == null)
                return settings;

            foreach (var row in settingsTable.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 2)
                {
                    var settingName = cells[0].GetTextContent();
                    var settingValue = cells[1].GetTextContent();

                    switch (settingName)
                    {
                        case "Запись демо":
                            settings.RecordingDemo = settingValue == "Вкл.";
                            break;
                        case "Герой":
                            settings.Hero = settingValue == "Вкл.";
                            break;
                        case "Героиня":
                            settings.Heroine = settingValue == "Вкл.";
                            break;
                        case "Класс по умолчанию":
                            settings.DefaultClass = settingValue;
                            break;
                        case "Язык":
                            settings.Language = settingValue;
                            break;
                        case "Туторы":
                            settings.Tutorials = settingValue == "Вкл.";
                            break;
                    }
                }
            }

            return settings;
        }

        private static PlayerBasicStats ParseBasicStats(IDocument document)
        {
            var stats = new PlayerBasicStats();

            var statsSection = document.QuerySelector(".smallstat.box.mobileHalf.span6 .title");
            if (statsSection == null || statsSection.TextContent != "Основная статистика")
                return stats;

            var statsTable = statsSection.ParentElement?.QuerySelector(".table-responsive table");
            if (statsTable == null)
                return stats;

            foreach (var row in statsTable.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 2)
                {
                    var label = cells[0].GetTextContent();
                    var valueCell = cells.Length > 2 ? cells[2] : cells[1];

                    switch (label)
                    {
                        case "Нож":
                            var knifeImg = row.QuerySelector("img");
                            if (knifeImg != null)
                                stats.Knife = knifeImg.GetAttribute("title");
                            break;

                        case "EXP":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int exp))
                                stats.Exp = exp;
                            break;

                        case "Следующий уровень":
                            var progressText = valueCell.TextContent;
                            if (!string.IsNullOrEmpty(progressText))
                            {
                                var match = NextLevelRegex().Match(progressText);

                                if (match.Success && int.TryParse(match.Groups[1].Value.Replace(" ", ""), out int nextLevel))
                                    stats.NextLevel = nextLevel;

                                var remainingMatch = RemainingTimeRegex().Match(progressText);

                                if (remainingMatch.Success && int.TryParse(remainingMatch.Groups[1].Value.Replace(" ", ""), out int expToNext))
                                    stats.ExpToNextLevel = expToNext;
                            }
                            break;

                        case "Деньги":
                            var moneyText = valueCell.TextContent;
                            if (int.TryParse(moneyText.Replace("$", "").Replace(" ", ""), out int money))
                                stats.Money = money;
                            break;

                        case "Аммо":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int ammo))
                                stats.Ammo = ammo;
                            break;

                        case "Денежные ключи":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int moneyKeys))
                                stats.MoneyKeys = moneyKeys;
                            break;

                        case "Аммо ключи":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int ammoKeys))
                                stats.AmmoKeys = ammoKeys;
                            break;

                        case "Нанес урона":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int damage))
                                stats.Damage = damage;
                            break;

                        case "Был лучшим":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int mvps))
                                stats.MVPs = mvps;
                            break;

                        case "Ассистов":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int assists))
                                stats.Assists = assists;
                            break;

                        case "Самоубийств":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int suicides))
                                stats.Suicides = suicides;
                            break;

                        case "Смертей":
                            if (int.TryParse(valueCell.TextContent.Replace(" ", ""), out int deaths))
                                stats.Deaths = deaths;
                            break;

                        case "У/С":
                            var kdText = valueCell.GetTextContent();
                            if (decimal.TryParse(kdText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kdRatio))
                                stats.KillDeathRatio = kdRatio;
                            break;
                    }
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
                if (titleElement?.GetTextContent() != "Достижения")
                    continue;

                var table = block.QuerySelector(".table-responsive table");
                if (table == null)
                    continue;

                foreach (var row in table.GetTableRows())
                {
                    var cells = row.GetTableCells();
                    if (cells.Length < 2) continue;

                    var label = cells[0].GetTextContent();
                    var valueText = cells[1].GetTextContent();
                    var cleanValue = valueText.Replace(" ", "");

                    switch (label)
                    {
                        case "Класс зомби":
                            progress.Class = valueText;
                            break;

                        case "Нанес урона (за зм)":
                            if (int.TryParse(cleanValue, out int damageByZombie))
                                progress.DamageByZombie = damageByZombie;
                            break;

                        case "Заразил":
                            if (int.TryParse(cleanValue, out int infects))
                                progress.Infects = infects;
                            break;

                        case "Был заражен":
                            if (int.TryParse(cleanValue, out int wasInfected))
                                progress.WasInfected = wasInfected;
                            break;

                        case "Был первым зомби":
                            if (int.TryParse(cleanValue, out int wasFirstZm))
                                progress.WasFirstZm = wasFirstZm;
                            break;

                        case "Был немезисом":
                            if (int.TryParse(cleanValue, out int wasNemesis))
                                progress.WasNemesis = wasNemesis;
                            break;

                        case "Был выжившим":
                            if (int.TryParse(cleanValue, out int wasSurvivor))
                                progress.WasSurvivor = wasSurvivor;
                            break;

                        case "Был героем":
                            if (int.TryParse(cleanValue, out int wasHero))
                                progress.WasHero = wasHero;
                            break;

                        case "Был героиней":
                            if (int.TryParse(cleanValue, out int wasHeroine))
                                progress.WasHeroine = wasHeroine;
                            break;

                        case "Убил зомби":
                            if (int.TryParse(cleanValue, out int zombieKills))
                                progress.ZombieKills = zombieKills;
                            break;

                        case "Убил людей":
                            if (int.TryParse(cleanValue, out int humanKills))
                                progress.HumanKills = humanKills;
                            break;

                        case "Убил немезисов":
                            if (int.TryParse(cleanValue, out int nemesisKills))
                                progress.NemesisKills = nemesisKills;
                            break;

                        case "Убил выживших":
                            if (int.TryParse(cleanValue, out int survivorKills))
                                progress.SurvivorKills = survivorKills;
                            break;

                        case "Убил боссов":
                            if (int.TryParse(cleanValue, out int bossKills))
                                progress.BossKills = bossKills;
                            break;
                    }
                }
                break;
            }

            return progress;
        }

        private static List<PlayerTempWeapon> ParseTempWeapons(IDocument document)
        {
            var tempWeapons = new List<PlayerTempWeapon>();

            var weaponsContainer = document.QuerySelector("#wpnhour");
            if (weaponsContainer == null)
                return tempWeapons;
            
            foreach (var card in weaponsContainer.QuerySelectorAll(".span4[style*='border: 1px solid #ddd']"))
            {
                var weapon = new PlayerTempWeapon();

                var labelElement = card.QuerySelector(".label");
                if (labelElement != null)
                    weapon.Main = labelElement.ClassList.Contains("label-online");

                var textSpan = card.QuerySelector(".row-fluid:last-child .span12");
                if (textSpan != null)
                {
                    var fullText = textSpan.GetTextContent();
                    fullText = fullText.Replace("&nbsp;", " ").Replace("\u00A0", " ").Trim();

                    var match = NameAndTimeRegex().Match(fullText);
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
            var weaponStats = new List<PlayerWeaponStatEntry>();

            var table = document.QuerySelector("#weapons table");
            if (table == null)
                return weaponStats;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 11)
                {
                    var weapon = new PlayerWeaponStatEntry
                    {
                        Name = cells[1].GetTextContent(),
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
            var modWeaponStats = new List<PlayerModWeaponStatEntry>();

            var table = document.QuerySelector("#modweaps table");
            if (table == null)
                return modWeaponStats;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 9)
                {
                    var weapon = new PlayerModWeaponStatEntry
                    {
                        Mod = GetModFromImage(cells[0]),
                        Name = cells[2].GetTextContent(),
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
            var zombieStats = new List<PlayerZombieStatEntry>();

            var table = document.QuerySelector("#zmstat");
            if (table == null)
                return zombieStats;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 10)
                {
                    var stat = new PlayerZombieStatEntry
                    {
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
            var grenades = new List<PlayerZombieGrenadesInfo>();

            var grenadeLink = document.QuerySelector("a[href*='#zmweapons']");
            if (grenadeLink?.TextContent.Contains("Гранаты зомби") != true)
                return grenades;

            var grenadesContainer = grenadeLink.Closest(".smallstat.box");
            if (grenadesContainer == null)
                return grenades;

            foreach (var block in grenadesContainer.QuerySelectorAll(".span6"))
            {
                var grenade = new PlayerZombieGrenadesInfo();

                var nameElement = block.QuerySelector(".charts-label1");
                if (nameElement != null)
                    grenade.Name = nameElement.GetTextContent();

                var valueElements = block.QuerySelectorAll(".value");
                var labelElements = block.QuerySelectorAll("p[style*='font-size: 14px;color: #c7cbd5;']");

                for (int i = 0; i < labelElements.Length; i++)
                {
                    var label = labelElements[i].GetTextContent();
                    var valueText = i < valueElements.Length ? valueElements[i].GetTextContent() : "0";

                    if (int.TryParse(valueText, out int value))
                    {
                        switch (label)
                        {
                            case "УРОН":
                                grenade.Damage = value;
                                break;
                            case "ЗАРАЖЕНИЙ":
                                grenade.Infects = value;
                                break;
                            case "БРОСКОВ":
                                grenade.Throws = value;
                                break;
                            case "УБИЙСТВ":
                                grenade.Kills = value;
                                break;
                        }
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
                if (src.Contains("hero")) return "Hero";
                if (src.Contains("heroine")) return "Heroine";
                if (src.Contains("surv")) return "Survivor";
                if (src.Contains("skill")) return "Skillmod";
            }
            return "Unknown";
        }

        private static string GetZombieClassFromImage(IElement cell)
        {
            var img = cell.QuerySelector("img");
            if (img != null)
            {
                var src = img.GetAttribute("src") ?? "";
                if (src.Contains("fast")) return "Fast";
                if (src.Contains("hunter")) return "Hunter";
                if (src.Contains("nemesis")) return "Nemesis";
                if (src.Contains("tesla")) return "Tesla";
                if (src.Contains("classic")) return "Classic";
                if (src.Contains("voodo")) return "Voodoo";
            }
            return "Unknown";
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"/(7656119\d+)/")]
        private static partial System.Text.RegularExpressions.Regex SteamIdRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"^(.+?)\s*\((.+?)\)$")]
        private static partial System.Text.RegularExpressions.Regex NameAndTimeRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"^.*?\s+\&nbsp;")]
        private static partial System.Text.RegularExpressions.Regex NickTextRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"(\d+(?:\s?\d+)*)\s+осталось")]
        private static partial System.Text.RegularExpressions.Regex RemainingTimeRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"(\d+(?:\s?\d+)*)")]
        private static partial System.Text.RegularExpressions.Regex NextLevelRegex();
    }
}
