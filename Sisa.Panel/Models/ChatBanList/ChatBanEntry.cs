namespace Sisa.Panel.Models.ChatBanList
{
    /// <summary>
    /// Запись с информацией о бане чата.
    /// </summary>
    public class ChatBanEntry
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Имя забаненного игрока.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Страна игрока.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Имя администратора, кем выдан бан.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Тип бана.
        /// </summary>
        public string BanType { get; set; }

        /// <summary>
        /// Длительность бана.
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// STEAM ID забаненного игрока.
        /// </summary>
        public string SteamId { get; set; }

        /// <summary>
        /// Профиль Steam.
        /// </summary>
        public string SteamProfile { get; set; }

        /// <summary>
        /// Дата выдачи бана.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Длительность бана.
        /// </summary>
        public string BanTime { get; set; }

        /// <summary>
        /// Когда истекает бан.
        /// </summary>
        public string Expires { get; set; }

        /// <summary>
        /// Количество предыдущих нарушений.
        /// </summary>
        public int PreviousViolations { get; set; }
    }
}
