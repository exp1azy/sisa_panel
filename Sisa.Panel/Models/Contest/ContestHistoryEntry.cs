namespace Sisa.Panel.Models.Contest
{
    /// <summary>
    /// Запись с информацией о конкурсе из архива.
    /// </summary>
    public class ContestHistoryEntry
    {
        /// <summary>
        /// Номер конкурса.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Дата завершения конкурса.
        /// </summary>
        public DateTime EndsAt { get; set; }

        /// <summary>
        /// Победитель конкурса.
        /// </summary>
        public string Winner { get; set; }

        /// <summary>
        /// Подарок.
        /// </summary>
        public string Gift { get; set; }

        /// <summary>
        /// Ссылка на фотографию игрока.
        /// </summary>
        public string Image { get; set; }
    }
}
