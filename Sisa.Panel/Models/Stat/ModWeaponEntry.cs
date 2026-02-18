namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией об оружии, доступным на модах.
    /// </summary>
    public class ModWeaponEntry
    {
        /// <summary>
        /// Кому принадлежит.
        /// </summary>
        public string BelongsTo { get; set; }

        /// <summary>
        /// Название оружия.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выстрелов.
        /// </summary>
        public int Shots { get; set; }

        /// <summary>
        /// Количество убийств зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Количество нанесенного урона по зомби.
        /// </summary>
        public long ZombieDamage { get; set; }

        /// <summary>
        /// Количество попаданий.
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// Количество ассистов.
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
        /// Коэффициент.
        /// </summary>
        public int Ratio { get; set; }

        /// <summary>
        /// Ссылка на изображение класса.
        /// </summary>
        public string ClassImage { get; set; }

        /// <summary>
        /// Ссылка на изображение оружия.
        /// </summary>
        public string WeaponImage { get; set; }
    }
}
