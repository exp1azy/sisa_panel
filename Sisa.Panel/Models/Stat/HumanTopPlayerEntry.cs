namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о лучшем игроке за человека.
    /// </summary>
    public class HumanTopPlayerEntry : TopPlayerEntry
    {
        /// <summary>
        /// Количество убийств зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Был заражен.
        /// </summary>
        public int? WasInfected { get; set; }
    }
}
