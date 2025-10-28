namespace Sisa.Panel.Models.ChatBanList
{
    public class ChatBanEntry
    {
        public string Id { get; set; }

        public string PlayerName { get; set; }

        public string Country { get; set; }

        public string AdminName { get; set; }

        public string BanType { get; set; }

        public string Duration { get; set; }

        public string SteamId { get; set; }

        public string AddedDate { get; set; }

        public string BanTime { get; set; }

        public string Expires { get; set; }

        public int PreviousViolations { get; set; }
    }
}
