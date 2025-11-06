namespace Sisa.Panel.Models.Clans
{
    /// <summary>
    /// Запись с информацией об участнике клана.
    /// </summary>
    public class ClanPlayerEntry
    {
        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Является ли участник администратором.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Является ли участник модератором.
        /// </summary>
        public bool IsModerator { get; set; }

        /// <summary>
        /// Уровень участника.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Время, проведенное в игре.
        /// </summary>
        public string Online { get; set; }

        /// <summary>
        /// Время, когда участник заходил в игру в последний раз.
        /// </summary>
        public DateTime LastActivity { get; set; }
    }
}
