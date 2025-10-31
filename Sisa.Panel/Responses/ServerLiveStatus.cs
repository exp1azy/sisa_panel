using Sisa.Panel.Models.Live;

namespace Sisa.Panel.Responses
{
    public class ServerLiveStatus
    {
        public ServerStatus Status { get; set; }

        public IReadOnlyList<PlayerLiveInfo> Players { get; set; }

        public IReadOnlyList<TeamSummary> Teams { get; set; }

        public ServerStatistics Statistics { get; set; }

        public IReadOnlyList<string> PreviousMaps { get; set; }
    }
}
