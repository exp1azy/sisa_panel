using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models.Live;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal partial class LiveStatusParser(IBrowsingContext context) : IParsable<ServerLiveStatus>
    {
        public async Task<ServerLiveStatus> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var liveStatus = new ServerLiveStatus
            {
                Status = ParseServerStatus(document),
                Players = ParsePlayers(document),
                Teams = ParseTeams(document),
                Statistics = ParseStatistics(document),
                PreviousMaps = ParsePreviousMaps(document)
            };

            return liveStatus;
        }

        private static ServerStatus ParseServerStatus(IDocument document)
        {
            var status = new ServerStatus();
            var serverInfoRows = document.QuerySelectorAll("table.table-condensed tbody tr");

            foreach (var row in serverInfoRows)
            {
                var cells = row.QuerySelectorAll("td").ToArray();
                if (cells.Length >= 2)
                {
                    var key = cells[0].TextContent.Trim();

                    switch (key)
                    {
                        case "Карта":
                            status.CurrentMap = cells[1].TextContent.Trim();
                            break;
                        case "Текущий мод":
                            status.CurrentMod = cells[1].TextContent.Trim();
                            break;
                        case "Игроков":
                            var playerText = cells[2].TextContent.Trim();
                            var playerMatch = TimeLeftRegex().Match(playerText);

                            if (playerMatch.Success)
                            {
                                status.PlayersOnline = int.Parse(playerMatch.Groups[1].Value);
                                status.MaxPlayers = int.Parse(playerMatch.Groups[2].Value);
                            }
                            break;
                        case "Осталось времени":
                            if (cells.Length < 3)
                                status.TimeLeft = cells[1].TextContent.Trim();
                            else
                                status.TimeLeft = cells[2].TextContent.Trim();
                            break;
                        case "Босс-раунд":
                            status.BossRoundAvailable = cells[1].TextContent.Trim() == "Доступен";
                            break;
                    }
                }
            }

            return status;
        }

        private static List<PlayerLiveInfo> ParsePlayers(IDocument document)
        {
            var players = new List<PlayerLiveInfo>();
            var playerRows = document.QuerySelectorAll("#scoreboard table tbody tr");

            foreach (var row in playerRows)
            {
                if (row.ClassList.Contains("zombie_head") || row.ClassList.Contains("humans_head"))
                    continue;

                var cells = row.QuerySelectorAll("td").ToArray();
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
                        player.PlayerName = nameLink.TextContent.Trim();

                    player.SteamId = cells[1].TextContent.Trim();

                    var fragsText = cells[2].TextContent.Trim();
                    var fragsMatch = FragsCountRegex().Match(fragsText);

                    if (fragsMatch.Success)
                    {
                        player.Kills = int.Parse(fragsMatch.Groups[1].Value);
                        player.Deaths = int.Parse(fragsMatch.Groups[2].Value);
                    }

                    player.PlayTime = cells[4].TextContent.Trim();

                    if (int.TryParse(cells[5].TextContent.Trim(), out int ping))
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
            var teamHeaders = document.QuerySelectorAll("#scoreboard table tbody tr.zombie_head, #scoreboard table tbody tr.humans_head");

            foreach (var header in teamHeaders)
            {
                var cells = header.QuerySelectorAll("td").ToArray();
                if (cells.Length >= 3)
                {
                    var teamSummary = new TeamSummary();

                    if (header.ClassList.Contains("zombie_head"))
                        teamSummary.Team = PlayerTeam.Zombie;
                    else if (header.ClassList.Contains("humans_head"))
                        teamSummary.Team = PlayerTeam.Human;

                    var teamText = cells[0].TextContent.Trim();
                    var playerCountMatch = PlayerCountRegex().Match(teamText);

                    if (playerCountMatch.Success)
                        teamSummary.PlayerCount = int.Parse(playerCountMatch.Groups[1].Value);

                    var roundsText = cells[1].TextContent.Trim();

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
                HourlyActivity = new Dictionary<string, int>(),
                MonthlyActivity = new Dictionary<string, int>()
            };

            var scriptElements = document.QuerySelectorAll("script");
            foreach (var script in scriptElements)
            {
                var scriptContent = script.TextContent;
                if (scriptContent.Contains("chart3Dat") && scriptContent.Contains("data :"))
                {
                    var matches = ActivityRegex().Matches(scriptContent);
                    foreach (System.Text.RegularExpressions.Match match in matches)
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
                    var matches = ActivityRegex().Matches(scriptContent);

                    foreach (System.Text.RegularExpressions.Match match in matches)
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
            var mapElements = document.QuerySelectorAll("#lastm .box-content .title");

            foreach (var mapElement in mapElements)
            {
                var mapName = mapElement.TextContent.Trim();

                if (!string.IsNullOrEmpty(mapName))
                    previousMaps.Add(mapName);
            }

            return previousMaps;
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"(\d+)\s*/\s*(\d+)")]
        private static partial System.Text.RegularExpressions.Regex TimeLeftRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"(\d+)\s+игроков")]
        private static partial System.Text.RegularExpressions.Regex PlayerCountRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"(\d+)\s*:\s*(\d+)")]
        private static partial System.Text.RegularExpressions.Regex FragsCountRegex();

        [System.Text.RegularExpressions.GeneratedRegex(@"{x:\s*'([^']+)',\s*y:\s*(\d+)}")]
        private static partial System.Text.RegularExpressions.Regex ActivityRegex();
    }
}
