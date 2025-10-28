using Sisa.Panel.Models.BanList;

namespace Sisa.Panel.Responses
{
    public class BanList
    {
        public IList<BanEntry> Bans { get; set; }

        public int TotalBans { get; set; }

        public int ActiveBans { get; set; }

        public int TotalDemos { get; set; }
    }
}
