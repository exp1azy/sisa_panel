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
            var clans = await client.GetClansListAsync();

            ;
        }
    }
}
