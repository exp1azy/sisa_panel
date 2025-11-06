namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией об оружии.
    /// </summary>
    public class WeaponEntry
    {
        /// <summary>
        /// Идентификатор оружия.
        /// </summary>
        public int Wid { get; set; }

        /// <summary>
        /// Название оружия.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выстрелов.
        /// </summary>
        public int Shots { get; set; }

        /// <summary>
        /// Количество убитых зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Количество нанесенного урона зомби.
        /// </summary>
        public long ZombieDamage { get; set; }

        /// <summary>
        /// Количество попаданий.
        /// </summary>
        public int Hits { get; set; }

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
        /// Количество нанесенного урона боссу.
        /// </summary>
        public int BossDamage { get; set; }

        /// <summary>
        /// Количество убийств босса.
        /// </summary>
        public int BossKills { get; set; }

        /// <summary>
        /// С какого уровня доступно.
        /// </summary>
        public int AvailableFromLevel { get; set; }

        /// <summary>
        /// Стоимость.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Коэффициент.
        /// </summary>
        public int Ratio { get; set; }
    }
}
