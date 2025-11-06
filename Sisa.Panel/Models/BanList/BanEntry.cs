namespace Sisa.Panel.Models.BanList
{
    /// <summary>
    /// Запись с информацией о бане.
    /// </summary>
    public class BanEntry
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата выдачи бана.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Имя забаненного игрока.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Страна игрока.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Имя администратора, которым выдан бан.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Причина бана.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Длительность бана.
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Наличие демо.
        /// </summary>
        public bool HasDemo { get; set; }

        /// <summary>
        /// Сервер, где забанен игрок.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Является ли игрок Steam пользователем.
        /// </summary>
        public bool IsSteamUser { get; set; }

        /// <summary>
        /// Профиль Steam.
        /// </summary>
        public string SteamProfile { get; set; }

        /// <summary>
        /// STEAM ID.
        /// </summary>
        public string SteamId { get; set; }

        /// <summary>
        /// Когда истекает бан.
        /// </summary>
        public string ExpiresDate { get; set; }

        /// <summary>
        /// Предыдущие нарушения.
        /// </summary>
        public int PreviousViolations { get; set; }

        /// <summary>
        /// Откуда скачан клиент игры.
        /// </summary>
        public string Client { get; set; }
    }
}
