using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Статистика топ игроков за зомби.
    /// </summary>
    public class ZombieTopPlayersStats
    {
        /// <summary>
        /// Статистика за классика.
        /// </summary>
        public ZombieClassInfo Classic { get; set; }

        /// <summary>
        /// Статистика за быструю.
        /// </summary>
        public ZombieClassInfo Fast { get; set; }

        /// <summary>
        /// Статистика за доктора.
        /// </summary>
        public ZombieClassInfo Healer { get; set; }

        /// <summary>
        /// Статистика за большого.
        /// </summary>
        public ZombieClassInfo Big { get; set; }

        /// <summary>
        /// Статистика за шамана.
        /// </summary>
        public ZombieClassInfo Voodo { get; set; }

        /// <summary>
        /// Статистика за хищника.
        /// </summary>
        public ZombieClassInfo Hunter { get; set; }

        /// <summary>
        /// Статистика за теслу.
        /// </summary>
        public ZombieClassInfo Tesla { get; set; }

        /// <summary>
        /// Статистика за немезиса.
        /// </summary>
        public ZombieClassInfo Nemesis { get; set; }
    }
}
