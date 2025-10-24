using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class BanList
    {
        public ICollection<BanEntry> Bans { get; set; }

        public int TotalBans { get; set; }

        public int ActiveBans { get; set; }

        public int TotalDemos { get; set; }
    }
}
