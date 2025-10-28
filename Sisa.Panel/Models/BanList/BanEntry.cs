namespace Sisa.Panel.Models.BanList
{
    public class BanEntry
    {
        public int BanId { get; set; }

        public string Date { get; set; }

        public string PlayerName { get; set; }

        public string Country { get; set; }

        public string AdminName { get; set; }

        public string Reason { get; set; }

        public string Duration { get; set; }

        public bool HasDemo { get; set; }

        public string Server { get; set; }

        public bool IsSteamUser { get; set; }

        public string SteamProfile { get; set; }

        public string SteamId { get; set; }

        public string AddedDate { get; set; }

        public string ExpiresDate { get; set; }

        public int PreviousViolations { get; set; }

        public string BanType { get; set; }
    }
}
