using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Live;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class LiveStatusParser(IBrowsingContext context) : IParser<ServerLiveStatus>
    {
        public async Task<ServerLiveStatus> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            return new ServerLiveStatus
            {
                Status = ParseServerStatus(document),
                Players = ParsePlayers(document),
                Teams = ParseTeams(document),
                Statistics = ParseStatistics(document),
                PreviousMaps = ParsePreviousMaps(document)
            };
        }

        private static ServerStatus ParseServerStatus(IDocument document)
        {
            var status = new ServerStatus();

            var table = document.QuerySelector("table.table-condensed");
            if (table == null)
                return status;

            foreach (var row in table.GetTableRows())
            {
                var cells = row.GetTableCells();
                if (cells.Length >= 2)
                {
                    switch (cells[0].GetTextContent())
                    {
                        case "Карта":
                            status.CurrentMap = cells[1].GetTextContent();
                            break;
                        case "Текущий мод":
                            status.CurrentMod = cells[1].GetTextContent();
                            break;
                        case "Игроков":
                            var playerText = cells[2].GetTextContent();
                            var playerMatch = ParserRegex.TimeLeftPattern().Match(playerText);

                            if (playerMatch.Success)
                            {
                                status.PlayersOnline = int.Parse(playerMatch.Groups[1].Value);
                                status.MaxPlayers = int.Parse(playerMatch.Groups[2].Value);
                            }
                            break;
                        case "Осталось времени":
                            if (cells.Length < 3)
                                status.TimeLeft = cells[1].GetTextContent();
                            else
                                status.TimeLeft = cells[2].GetTextContent();
                            break;
                        case "Босс-раунд":
                            status.BossRoundAvailable = cells[1].GetTextContent() == "Доступен";
                            break;
                    }
                }
            }

            return status;
        }

        private static List<PlayerLiveInfo> ParsePlayers(IDocument document)
        {
            var players = new List<PlayerLiveInfo>();

            var table = document.QuerySelector("#scoreboard table");
            if (table == null)
                return players;

            foreach (var row in table.GetTableRows())
            {
                if (row.ClassList.Contains("zombie_head") || row.ClassList.Contains("humans_head"))
                    continue;

                var cells = row.GetTableCells();
                if (cells.Length >= 7)
                {
                    var player = new PlayerLiveInfo();

                    if (row.ClassList.Contains("zombie"))
                        player.Team = PlayerTeam.Zombie;
                    else if (row.ClassList.Contains("humans"))
                        player.Team = PlayerTeam.Human;

                    var playerCell = cells[0];
                    var flagImg = playerCell.QuerySelector("img");

                    if (flagImg != null)
                        player.Country = flagImg.GetAttribute("alt") ?? "Unknown";

                    var nameLink = playerCell.QuerySelector("a");

                    if (nameLink != null)
                        player.PlayerName = nameLink.GetTextContent();

                    player.SteamId = cells[1].GetTextContent();

                    var fragsText = cells[2].GetTextContent();
                    var fragsMatch = ParserRegex.FragsPattern().Match(fragsText);

                    if (fragsMatch.Success)
                    {
                        player.Kills = int.Parse(fragsMatch.Groups[1].Value);
                        player.Deaths = int.Parse(fragsMatch.Groups[2].Value);
                    }

                    player.PlayTime = cells[4].GetTextContent();

                    if (int.TryParse(cells[5].GetTextContent(), out int ping))
                        player.Ping = ping;

                    var levelElement = cells[6].QuerySelector("span.lvlx");
                    if (levelElement != null)
                    {
                        var levelText = levelElement.TextContent;

                        if (int.TryParse(levelText, out int level))
                            player.Level = level;
                    }

                    players.Add(player);
                }
            }

            return players;
        }

        private static List<TeamSummary> ParseTeams(IDocument document)
        {
            var teams = new List<TeamSummary>();

            foreach (var header in document.QuerySelectorAll("#scoreboard table tbody tr.zombie_head, #scoreboard table tbody tr.humans_head"))
            {
                var cells = header.GetTableCells();
                if (cells.Length >= 3)
                {
                    var teamSummary = new TeamSummary();

                    if (header.ClassList.Contains("zombie_head"))
                        teamSummary.Team = PlayerTeam.Zombie;
                    else if (header.ClassList.Contains("humans_head"))
                        teamSummary.Team = PlayerTeam.Human;

                    var teamText = cells[0].GetTextContent();
                    var playerCountMatch = ParserRegex.PlayerCountPattern().Match(teamText);

                    if (playerCountMatch.Success)
                        teamSummary.PlayerCount = int.Parse(playerCountMatch.Groups[1].Value);

                    var roundsText = cells[1].GetTextContent();

                    if (int.TryParse(roundsText, out int rounds))
                        teamSummary.WonRounds = rounds;

                    teams.Add(teamSummary);
                }
            }

            return teams;
        }

        private static ServerStatistics ParseStatistics(IDocument document)
        {
            var statistics = new ServerStatistics
            {
                HourlyActivity = [],
                MonthlyActivity = []
            };

            foreach (var script in document.QuerySelectorAll("script"))
            {
                var scriptContent = script.TextContent;
                if (scriptContent.Contains("chart3Dat") && scriptContent.Contains("data :"))
                {
                    foreach (System.Text.RegularExpressions.Match match in ParserRegex.ActivityPattern().Matches(scriptContent))
                    {
                        if (match.Groups.Count == 3)
                        {
                            var timestamp = match.Groups[1].Value;
                            var playerCount = int.Parse(match.Groups[2].Value);

                            if (DateTime.TryParse(timestamp, out DateTime dateTime))
                            {
                                var timeKey = dateTime.ToString("HH:mm");
                                statistics.HourlyActivity[timeKey] = playerCount;
                            }
                            else
                            {
                                statistics.HourlyActivity[timestamp] = playerCount;
                            }
                        }
                    }
                }

                if (scriptContent.Contains("chart4Dat") && scriptContent.Contains("data:"))
                {
                    foreach (System.Text.RegularExpressions.Match match in ParserRegex.ActivityPattern().Matches(scriptContent))
                    {
                        if (match.Groups.Count == 3)
                        {
                            var timestamp = match.Groups[1].Value;
                            statistics.MonthlyActivity[timestamp] = int.Parse(match.Groups[2].Value);
                        }
                    }
                }
            }

            return statistics;
        }

        private static List<string> ParsePreviousMaps(IDocument document)
        {
            var previousMaps = new List<string>();

            foreach (var mapElement in document.QuerySelectorAll("#lastm .box-content .title"))
            {
                var mapName = mapElement.GetTextContent();

                if (!string.IsNullOrEmpty(mapName))
                    previousMaps.Add(mapName);
            }

            return previousMaps;
        }
    }
}
