namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о статистике оружия игрока.
    /// </summary>
    public class PlayerWeaponStatEntry
    {
        /// <summary>
        /// Название оружия.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выстрелов.
        /// </summary>
        public int Shots { get; set; }

        /// <summary>
        /// Количество попаданий.
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// Точность.
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// Количество убийств зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Количество нанесенного урона зомби.
        /// </summary>
        public int ZombieDamage { get; set; }

        /// <summary>
        /// Ассистов.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Был лучшим.
        /// </summary>
        public int MVPs { get; set; }

        /// <summary>
        /// Уровней набито.
        /// </summary>
        public int Levels { get; set; }

        /// <summary>
        /// Количество урона по боссу.
        /// </summary>
        public int BossDamage { get; set; }

        /// <summary>
        /// Количество убийств босса.
        /// </summary>
        public int BossKills { get; set; }
    }
}
