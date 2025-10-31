namespace Sisa.Panel.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SisaPanelClient();

            // var bans = await client.GetBanListAsync(10, 50);
            // var chatBans = await client.GetChatBanListAsync(1, 20);
            //var chatLog = await client.GetChatLogAsync();
            //var admins = await client.GetAdminsListAsync();
            //var liveStatus = await client.GetLiveStatusAsync();
            //var clans = await client.GetClanListAsync();
            //var clan = await client.GetClanInfoAsync(clans.Clans.ToList().First().Id);
            //var playerStats = await client.GetPlayerStatsAsync(100, 3);
            //var weapons = await client.GetWeaponStatsAsync();
            //var stats = await client.GetHumanTopPlayersStatsAsync();
            //var stats = await client.GetZombieTopPlayersAsync();
            //var maps = await client.GetMapStatsAsync();
            var player = await client.GetPlayerInfo(26701);
        }
    }
}
