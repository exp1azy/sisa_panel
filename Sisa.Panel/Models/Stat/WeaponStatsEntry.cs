namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о статистике игрока по оружию.
    /// </summary>
    public class WeaponStatsEntry
    {
        /// <summary>
        /// Позиция игрока в рейтинге.
        /// </summary>
        public int RatingPosition { get; set; }

        /// <summary>
        /// Страна.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выстрелов.
        /// </summary>
        public int Shots { get; set; }

        /// <summary>
        /// Количество попаданий.
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// Точность.
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// Количество убитых зомби.
        /// </summary>
        public int ZmKills { get; set; }

        /// <summary>
        /// Количество нанесенного урона зомби.
        /// </summary>
        public int ZmDamage { get; set; }

        /// <summary>
        /// Ассистов.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Количество набитых лучших игроков.
        /// </summary>
        public int MVPs { get; set; }

        /// <summary>
        /// Количество набитых уровней.
        /// </summary>
        public int Levels { get; set; }

        /// <summary>
        /// Количество нанесенного урона по боссу.
        /// </summary>
        public int BossDamage { get; set; }

        /// <summary>
        /// Количество убитых боссов.
        /// </summary>
        public int BossKills { get; set; }

        /// <summary>
        /// Ссылка на фотографию игрока.
        /// </summary>
        public string Image { get; set; }
    }
}
