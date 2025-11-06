using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Статистика топ игроков за людей.
    /// </summary>
    public class HumanTopPlayersStats
    {
        /// <summary>
        /// Статистика за людей.
        /// </summary>
        public HumanClassInfo Human { get; set; }

        /// <summary>
        /// Статистика за выживших.
        /// </summary>
        public HumanClassInfo Survivor { get; set; }

        /// <summary>
        /// Статистика за героиню.
        /// </summary>
        public HumanClassInfo Heroine { get; set; }

        /// <summary>
        /// Статистика за продвинутый мод.
        /// </summary>
        public HumanClassInfo AdvancedMod { get; set; }

        /// <summary>
        /// Статистика за женский мод.
        /// </summary>
        public HumanClassInfo WomanMod { get; set; }

        /// <summary>
        /// Статистика за героя.
        /// </summary>
        public HumanClassInfo Hero { get; set; }
    }
}
