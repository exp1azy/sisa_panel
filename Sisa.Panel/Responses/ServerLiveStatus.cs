using Sisa.Panel.Models.Live;

namespace Sisa.Panel.Responses
{
    public class ServerLiveStatus
    {
        public ServerStatus Status { get; set; }

        public IList<PlayerInfo> Players { get; set; }

        public IList<TeamSummary> Teams { get; set; }

        public ServerStatistics Statistics { get; set; }

        public IList<string> PreviousMaps { get; set; }
    }
}
