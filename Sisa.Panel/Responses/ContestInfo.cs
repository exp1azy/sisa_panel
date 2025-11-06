using Sisa.Panel.Models.Contest;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Информация о конкурсе.
    /// </summary>
    public class ContestInfo
    {
        /// <summary>
        /// Последние победители конкурса.
        /// </summary>
        public IReadOnlyList<LastWinner> LastContestWinners { get; set; }

        /// <summary>
        /// Текущие участники конкурса.
        /// </summary>
        public IReadOnlyList<ContestParticipant> CurrentContestParticipants { get; set; }
    }
}
