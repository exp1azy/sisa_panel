using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Clans;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class ClanInfoParser(IBrowsingContext context) : IParser<ClanInfo>
    {
        public async Task<ClanInfo> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new ClanInfo
            {
                GeneralInfo = ParseGeneralInfo(document),
                LastActions = ParseLastActions(document),
                Members = ParseMembers(document)
            };
        }

        private static ClanGeneralInfo ParseGeneralInfo(IDocument document)
        {
            var generalInfo = new ClanGeneralInfo();

            var clanImageElement = document.QuerySelector(".box-content .row-fluid .span12 center img");
            generalInfo.ClanImage = clanImageElement.ExtractRelativeImageUrl();

            var spans = document.QuerySelectorAll("span");
            var actions = new List<string>(spans.Length);

            foreach (var span in spans)
            {
                if (string.IsNullOrEmpty(span.ClassName))
                    continue;

                var isAction = span.ClassName.StartsWithOrdinal("label");

                if (!isAction)
                    continue;

                var title = span.GetAttribute("title")?.Trim();

                if (string.IsNullOrEmpty(title))
                    continue;

                actions.Add(title);
            }

            generalInfo.Actions = actions;

            var stats = document.QuerySelectorAll(".smallstat .value.count");
            var statValues = stats.Select(s => s.TextContent).ToArray();

            if (statValues.Length >= 9)
            {
                if (int.TryParse(statValues[0], out int online))
                    generalInfo.Online = online;

                if (int.TryParse(statValues[1], out int bank))
                    generalInfo.Bank = bank;

                if (int.TryParse(statValues[2], out int ammo))
                    generalInfo.Ammo = ammo;

                if (int.TryParse(statValues[3], out int zombieKills))
                    generalInfo.ZombieKills = zombieKills;

                if (int.TryParse(statValues[4], out int nemesisKills))
                    generalInfo.NemesisKills = nemesisKills;

                if (int.TryParse(statValues[5], out int bossKills))
                    generalInfo.BossKills = bossKills;

                if (int.TryParse(statValues[6], out int humanKills))
                    generalInfo.HumanKills = humanKills;

                if (int.TryParse(statValues[7], out int survivorKills))
                    generalInfo.SurvivorKills = survivorKills;

                if (int.TryParse(statValues[8], out int infections))
                    generalInfo.Infections = infections;
            }

            return generalInfo;
        }

        private static List<ClanLastActionEntry> ParseLastActions(IDocument document)
        {
            var actionTable = document.QuerySelector("table.table-condensed");
            if (actionTable == null) return [];

            var rows = actionTable.GetTableRows();
            var actions = new List<ClanLastActionEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 4)
                {
                    var actionEntry = new ClanLastActionEntry();

                    var kindIcon = cells[0].QuerySelector("i");
                    var actionIcon = cells[1].QuerySelector("i");

                    if (kindIcon != null)
                        actionEntry.Kind = GetActionKindFromIcon(kindIcon.ClassList);

                    if (actionIcon != null)
                        actionEntry.Action = GetActionTypeFromIcon(actionIcon.ClassList);

                    actionEntry.Member = cells[2].TextContent;

                    var dateText = cells[3].TextContent;
                    actionEntry.Date = dateText.ParseToDateOnly();

                    actions.Add(actionEntry);
                }
            }

            return actions;
        }

        private static string GetActionKindFromIcon(ITokenList classList)
        {
            if (classList.Contains("fa-user")) return "Игрок";
            if (classList.Contains("fa-money")) return "Банк";
            if (classList.Contains("fa-star-o")) return "Модератор";
            if (classList.Contains("fa-star")) return "Администратор";
            return "Неизвестно";
        }

        private static string GetActionTypeFromIcon(ITokenList classList)
        {
            if (classList.Contains("fa-user-times")) return "Вышел из клана";
            if (classList.Contains("fa-user-plus")) return "Принял заявку";
            if (classList.Contains("fa-paper-plane")) return "Отправил инвайт";
            if (classList.Contains("fa-plus")) return "Игрок пополнил банк";
            if (classList.Contains("fa-minus")) return "Игрок снял деньги";
            return "Неизвестное действие";
        }

        private static List<ClanPlayerEntry> ParseMembers(IDocument document)
        {
            var memberTable = document.QuerySelectorAll("table.table-condensed")[^1];
            if (memberTable == null) return [];

            var rows = memberTable.GetTableRows();
            var members = new List<ClanPlayerEntry>(rows.Length);

            foreach (var row in rows)
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 5)
                {
                    var member = new ClanPlayerEntry();

                    var nameCell = cells[1];
                    member.Name = nameCell.TextContent.Trim();

                    var adminIcon = nameCell.QuerySelector("i.fa-star");
                    member.IsAdmin = adminIcon != null;

                    var modIcon = nameCell.QuerySelector("i.fa-star-o");
                    member.IsModerator = modIcon != null;

                    var levelSpan = cells[2].QuerySelector("span.lvlx");
                    if (levelSpan != null)
                    {
                        var levelText = levelSpan.TextContent;

                        if (int.TryParse(levelText, out int level))
                            member.Level = level;
                    }

                    member.Online = cells[3].TextContent;

                    var lastActivityText = cells[4].TextContent;
                    member.LastActivity = lastActivityText.ParseToDateTime();

                    members.Add(member);
                }
            }

            return members;
        }
    }
}
