namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о лучшем игроке за зомби.
    /// </summary>
    public class ZombieTopPlayerEntry : TopPlayerEntry
    {
        /// <summary>
        /// Количество заражений.
        /// </summary>
        public int? Infects { get; set; }

        /// <summary>
        /// Количество убитых людей.
        /// </summary>
        public int HumanKills { get; set; }

        /// <summary>
        /// Количество убитых выживших.
        /// </summary>
        public int SurvivorKills { get; set; }
    }
}
