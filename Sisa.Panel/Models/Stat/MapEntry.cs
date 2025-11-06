namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о карте.
    /// </summary>
    public class MapEntry
    {
        /// <summary>
        /// Название карты.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество игр на карте.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Количество побед людей.
        /// </summary>
        public int HumanWins { get; set; }

        /// <summary>
        /// Количество побед зомби.
        /// </summary>
        public int ZombieWins { get; set; }

        /// <summary>
        /// Ничьи.
        /// </summary>
        public int Draws { get; set; }
    }
}
