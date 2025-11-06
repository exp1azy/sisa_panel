namespace Sisa.Panel.Models.Live
{
    /// <summary>
    /// Информация о команде.
    /// </summary>
    public class TeamSummary
    {
        /// <summary>
        /// Команда (люди или зомби).
        /// </summary>
        public PlayerTeam Team { get; set; }

        /// <summary>
        /// Количество игроков в команде.
        /// </summary>
        public int PlayerCount { get; set; }

        /// <summary>
        /// Количество побед.
        /// </summary>
        public int WonRounds { get; set; }
    }
}
