using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class ClanInfo
    {
        public ClanGeneralInfo GeneralInfo { get; set; }

        public ICollection<ClanLastActionEntry> LastActions { get; set; }

        public ICollection<ClanPlayerEntry> Members { get; set; }
    }
}
