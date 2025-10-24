using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class ServerLiveStatus
    {
        public ServerStatus Status { get; set; }

        public ICollection<PlayerInfo> Players { get; set; }

        public ICollection<TeamSummary> Teams { get; set; }

        public ServerStatistics Statistics { get; set; }

        public ICollection<string> PreviousMaps { get; set; }
    }
}
