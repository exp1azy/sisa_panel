namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Запись с информацией о лучшем игроке.
    /// </summary>
    public class TopPlayerEntry
    {
        /// <summary>
        /// Позиция в рейтинге.
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
        /// Нанесенный урон.
        /// </summary>
        public long Damage { get; set; }

        /// <summary>
        /// Количество смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Количество игр.
        /// </summary>
        public int? Games { get; set; }
    }
}
