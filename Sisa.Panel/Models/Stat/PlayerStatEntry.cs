namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о статистике игроке=а.
    /// </summary>
    public class PlayerStatEntry
    {
        /// <summary>
        /// Идентификатор игрока.
        /// </summary>
        public int Uid { get; set; }

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
        public float KillDeathRatio { get; set; }

        /// <summary>
        /// Был лучшим.
        /// </summary>
        public int MVPs { get; set; }

        /// <summary>
        /// Нож.
        /// </summary>
        public string Knife { get; set; }
    }
}
