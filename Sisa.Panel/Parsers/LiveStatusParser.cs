using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.Live;
using Sisa.Panel.Parsers.Interfaces;
using Sisa.Panel.Responses;
using System.Text.RegularExpressions;

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

                if (cells.Length < 2)
                    continue;

                if (cells[0].TextContent.EqualsOrdinal("Карта"))
                {
                    status.CurrentMap = cells[1].TextContent;
                }
                else if (cells[0].TextContent.EqualsOrdinal("Текущий мод"))
                {
                    status.CurrentMod = cells[1].TextContent;
                }
                else if (cells[0].TextContent.EqualsOrdinal("Игроков"))
                {
                    var playerText = cells[2].TextContent;
                    var playerMatch = ParserRegex.TimeLeftPattern.Match(playerText);

                    if (playerMatch.Success)
                    {
                        status.PlayersOnline = int.Parse(playerMatch.Groups[1].Value);
                        status.MaxPlayers = int.Parse(playerMatch.Groups[2].Value);
                    }
                }
                else if (cells[0].TextContent.EqualsOrdinal("Осталось времени"))
                {
                    if (cells.Length < 3)
                        status.TimeLeft = cells[1].TextContent;
                    else
                        status.TimeLeft = cells[2].TextContent;
                }
                else if (cells[0].TextContent.EqualsOrdinal("Босс-раунд"))
                {
                    status.BossRoundAvailable = cells[1].TextContent.EqualsOrdinal("Доступен");
                }
            }

            return status;
        }

        private static List<PlayerLiveInfo> ParsePlayers(IDocument document)
        {
            var table = document.QuerySelector("#scoreboard table");
            if (table == null)
                return [];

            var rows = table.GetTableRows();
            var players = new List<PlayerLiveInfo>(rows.Length);

            foreach (var row in rows)
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
                    player.Country = playerCell.ExtractImgAltAttribute();
                    player.PlayerName = playerCell.ExtractLinkText().Trim();
                    player.SteamId = cells[1].TextContent;

                    var fragsText = cells[2].TextContent;
                    var fragsMatch = ParserRegex.FragsPattern.Match(fragsText);

                    if (fragsMatch.Success)
                    {
                        player.Kills = int.Parse(fragsMatch.Groups[1].Value);
                        player.Deaths = int.Parse(fragsMatch.Groups[2].Value);
                    }

                    player.PlayTime = cells[4].TextContent;

                    if (int.TryParse(cells[5].TextContent, out int ping))
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
            var headers = document.QuerySelectorAll("#scoreboard table tbody tr.zombie_head, #scoreboard table tbody tr.humans_head");
            var teams = new List<TeamSummary>(headers.Length);

            foreach (var header in headers)
            {
                var cells = header.GetTableCells();
                if (cells.Length >= 3)
                {
                    var teamSummary = new TeamSummary();

                    if (header.ClassList.Contains("zombie_head"))
                        teamSummary.Team = PlayerTeam.Zombie;
                    else if (header.ClassList.Contains("humans_head"))
                        teamSummary.Team = PlayerTeam.Human;

                    var teamText = cells[0].TextContent;
                    var playerCountMatch = ParserRegex.PlayerCountPattern.Match(teamText);

                    if (playerCountMatch.Success)
                        teamSummary.PlayerCount = int.Parse(playerCountMatch.Groups[1].Value);

                    var roundsText = cells[1].TextContent;

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

            foreach (var script in document.QuerySelectorAll("script"))
            {
                var scriptContent = script.TextContent;
                if (scriptContent.ContainsOrdinal("chart3Dat") && scriptContent.ContainsOrdinal("data :"))
                {
                    foreach (Match match in ParserRegex.ActivityPattern.Matches(scriptContent))
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

                if (scriptContent.ContainsOrdinal("chart4Dat") && scriptContent.ContainsOrdinal("data:"))
                {
                    foreach (Match match in ParserRegex.ActivityPattern.Matches(scriptContent))
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
            var elems = document.QuerySelectorAll("#lastm .box-content .title");
            var previousMaps = new List<string>(elems.Length);

            foreach (var mapElement in elems)
            {
                var mapName = mapElement.TextContent.Trim();

                if (!string.IsNullOrEmpty(mapName))
                    previousMaps.Add(mapName);
            }

            return previousMaps;
        }
    }
}
