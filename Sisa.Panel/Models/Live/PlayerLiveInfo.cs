namespace Sisa.Panel.Models.Live
{
    public class PlayerLiveInfo
    {
        public string PlayerName { get; set; }

        public string SteamId { get; set; }

        public string Country { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public string PlayTime { get; set; }

        public int Ping { get; set; }

        public int Level { get; set; }

        public PlayerTeam Team { get; set; }
    }
}
