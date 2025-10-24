namespace Sisa.Panel.Models
{
    public class ServerStatus
    {
        public string CurrentMap { get; set; }

        public string CurrentMod { get; set; }

        public int PlayersOnline { get; set; }

        public int MaxPlayers { get; set; }

        public string TimeLeft { get; set; }

        public bool BossRoundAvailable { get; set; }
    }
}
