namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о статистике игрока за зомби.
    /// </summary>
    public class PlayerZombieStatEntry
    {
        /// <summary>
        /// Класс зомби.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Позиция в рейтинге.
        /// </summary>
        public int RatingPosition { get; set; }

        /// <summary>
        /// Количество заражений.
        /// </summary>
        public int Infects { get; set; }

        /// <summary>
        /// Количество убитых людей.
        /// </summary>
        public int HumanKills { get; set; }

        /// <summary>
        /// Количество убитых выживших.
        /// </summary>
        public int SurvivorKills { get; set; }

        /// <summary>
        /// Нанесенный урон.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Количество смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Количество игр.
        /// </summary>
        public int Games { get; set; }

        /// <summary>
        /// Был первым зомби.
        /// </summary>
        public int WasFirstZm { get; set; }

        /// <summary>
        /// Количество самоубийств.
        /// </summary>
        public int Suicides { get; set; }
    }
}
