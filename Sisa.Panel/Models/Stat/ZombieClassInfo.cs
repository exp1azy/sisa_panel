namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Информация о классе зомби.
    /// </summary>
    public class ZombieClassInfo
    {
        /// <summary>
        /// Название класса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Здоровье.
        /// </summary>
        public int? Health { get; set; }

        /// <summary>
        /// Скорость.
        /// </summary>
        public int? Speed { get; set; }

        /// <summary>
        /// Отброс.
        /// </summary>
        public string? Knockback { get; set; }

        /// <summary>
        /// Заражений.
        /// </summary>
        public int? Infects { get; set; }

        /// <summary>
        /// Количество убитых людей.
        /// </summary>
        public int HumansKills { get; set; }

        /// <summary>
        /// Количество убитых выживших.
        /// </summary>
        public int SurvivorsKills { get; set; }

        /// <summary>
        /// Смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Игр.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Урон.
        /// </summary>
        public long Damage { get; set; }

        /// <summary>
        /// Первый зомби.
        /// </summary>
        public int? FirstZm { get; set; }

        /// <summary>
        /// Самоубийств.
        /// </summary>
        public int Suicides { get; set; }

        /// <summary>
        /// Ссылка на изображение класса зомби.
        /// </summary>
        public string ClassImage { get; set; }

        /// <summary>
        /// Топ-10 лучших игроков за зомби.
        /// </summary>
        public ZombieTopPlayerEntry[] Players { get; set; } = new ZombieTopPlayerEntry[10];
    }
}
