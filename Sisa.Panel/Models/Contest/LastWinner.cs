namespace Sisa.Panel.Models.Contest
{
    /// <summary>
    /// Информация о последнем победителе в конкурсе.
    /// </summary>
    public class LastWinner
    {
        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Steam профиль.
        /// </summary>
        public string SteamProfile { get; set; }

        /// <summary>
        /// Подарок.
        /// </summary>
        public string Gift { get; set; }
    }
}
