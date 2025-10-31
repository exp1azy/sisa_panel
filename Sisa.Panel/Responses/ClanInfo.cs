using Sisa.Panel.Models.Clans;

namespace Sisa.Panel.Responses
{
    public class ClanInfo
    {
        public ClanGeneralInfo GeneralInfo { get; set; }

        public IReadOnlyList<ClanLastActionEntry> LastActions { get; set; }

        public IReadOnlyList<ClanPlayerEntry> Members { get; set; }
    }
}
