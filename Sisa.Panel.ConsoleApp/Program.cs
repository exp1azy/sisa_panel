namespace Sisa.Panel.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SisaPanelClient();

            var bans = await client.GetBansAsync();
            var chatBans = await client.GetChatbansAsync(1, 20);
            var chatLog = await client.GetChatlogAsync(1, 200, new DateOnly(2023, 1, 1));
            var admins = await client.GetAdminsAsync();
            var liveStatus = await client.GetLiveAsync();
            var clans = await client.GetClansAsync();
            var clan = await client.GetClanAsync(clans[0].Id);
            var pStats = await client.GetStatsAsync();
            var weapons = await client.GetWeaponStatsAsync();
            var weapon = await client.GetWeaponStatsAsync(weapons.Weapons[0].Wid);
            var hStats = await client.GetHumanTopPlayersAsync();
            var zStats = await client.GetZombieTopPlayersAsync();
            var maps = await client.GetMapStatsAsync();
            var search = await client.SearchAsync("TBoPeHue");
            var player = await client.GetPlayerAsync(search[0].Uid);
            var contest = await client.GetContestAsync();
            var history = await client.GetContestHistoryAsync(2, 20);
        }
    }
}
