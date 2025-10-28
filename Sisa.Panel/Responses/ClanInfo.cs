using Sisa.Panel.Models.Clans;

namespace Sisa.Panel.Responses
{
    public class ClanInfo
    {
        public ClanGeneralInfo GeneralInfo { get; set; }

        public IList<ClanLastActionEntry> LastActions { get; set; }

        public IList<ClanPlayerEntry> Members { get; set; }
    }
}
