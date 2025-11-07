namespace Sisa.Panel.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SisaPanelClient();

            //var bans = await client.GetBansAsync();
            //var chatBans = await client.GetChatBansAsync(1, 20);
            //var chatLog = await client.GetChatLogAsync();
            //var admins = await client.GetAdminsAsync();
            //var liveStatus = await client.GetLiveStatusAsync();
            //var clans = await client.GetClansAsync();
            //var clan = await client.GetClanAsync(clans[0].Id);
            //var playerStats = await client.GetPlayerStatsAsync();
            //var weapons = await client.GetWeaponStatsAsync();
            //var hStats = await client.GetHumanTopPlayersAsync();
            //var zStats = await client.GetZombieTopPlayersAsync();
            //var maps = await client.GetMapStatsAsync();
            var player = await client.GetPlayerAsync(132582);
            var weapon = await client.GetWeaponStatsAsync(/*weapons.Weapons[0].Wid*/);
            var search = await client.SearchAsync("straw");
            var contest = await client.GetContestAsync();
            var history = await client.GetContestHistoryAsync(2, 20);
        }
    }
}
