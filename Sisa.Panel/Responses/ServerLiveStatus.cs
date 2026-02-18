using Sisa.Panel.Models.Live;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Статус сервера.
    /// </summary>
    public class ServerLiveStatus
    {
        /// <summary>
        /// Статус сервера.
        /// </summary>
        public ServerStatus Status { get; set; }

        /// <summary>
        /// Список игроков онлайн.
        /// </summary>
        public IReadOnlyList<PlayerLiveInfo> Players { get; set; }

        /// <summary>
        /// Статистика по командам.
        /// </summary>
        public IReadOnlyList<TeamSummary> Teams { get; set; }

        /// <summary>
        /// Статистика сервера.
        /// </summary>
        public ServerStatistics Statistics { get; set; }

        /// <summary>
        /// Предыдущие карты.
        /// </summary>
        public IReadOnlyList<LivePreviousMap> PreviousMaps { get; set; }
    }
}
