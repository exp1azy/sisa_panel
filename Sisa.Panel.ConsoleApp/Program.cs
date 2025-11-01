namespace Sisa.Panel.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SisaPanelClient();

            //var bans = await client.GetBanListAsync(10, 50);
            //var chatBans = await client.GetChatBanListAsync(1, 20);
            //var chatLog = await client.GetChatLogAsync();
            //var admins = await client.GetAdminListAsync();
            //var liveStatus = await client.GetLiveStatusAsync();
            //var clans = await client.GetClanListAsync();
            //var clan = await client.GetClanInfoAsync(clans[0].Id);
            var playerStats = await client.GetPlayerStatsAsync();
            var weapons = await client.GetWeaponStatsAsync();
            //var hStats = await client.GetHumanTopPlayersAsync();
            //var zStats = await client.GetZombieTopPlayersAsync();
            //var maps = await client.GetMapStatsAsync();
            var player = await client.GetPlayerInfo(playerStats[0].Uid);
            var weapon = await client.GetWeaponStatsAsync(weapons.Weapons[0].Wid);
            //var search = await client.SearchAsync("straw");
            //var contest = await client.GetContestParticipantsAsync();
            //var history = await client.GetContestHistoryAsync(2, 20);
        }
    }
}
