namespace Sisa.Panel.Models.Contest
{
    /// <summary>
    /// Информация об участнике конкурса.
    /// </summary>
    public class ContestParticipant
    {
        /// <summary>
        /// Страна участника.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата регистрации в конкурсе.
        /// </summary>
        public DateTime RegisteredAt { get; set; }

        /// <summary>
        /// Ссылка на фотографию игрока.
        /// </summary>
        public string Image { get; set; }
    }
}
