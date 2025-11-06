namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией об игроке, найденном в поиске.
    /// </summary>
    public class PlayerSearchEntry
    {
        /// <summary>
        /// Страна.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ранг.
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// Уровень.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Опыт.
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Количество убийств зомби.
        /// </summary>
        public int ZmKills { get; set; }

        /// <summary>
        /// Ассистов.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Соотношение убийств к смертям.
        /// </summary>
        public decimal KillDeathRatio { get; set; }

        /// <summary>
        /// Был лучшим.
        /// </summary>
        public int MVPs { get; set; }

        /// <summary>
        /// Онлайн.
        /// </summary>
        public string Online { get; set; }
    }
}
