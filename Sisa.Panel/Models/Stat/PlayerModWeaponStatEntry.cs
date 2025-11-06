namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о статистике оружия игрока, доступном на модах.
    /// </summary>
    public class PlayerModWeaponStatEntry
    {
        /// <summary>
        /// Название мода.
        /// </summary>
        public string Mod { get; set; }

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
        /// Ассистов получено.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Уровней набито.
        /// </summary>
        public int Levels { get; set; }
    }
}
