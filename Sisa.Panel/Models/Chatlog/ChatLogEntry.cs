namespace Sisa.Panel.Models.Chatlog
{
    /// <summary>
    /// Запись журнала чата.
    /// </summary>
    public class ChatLogEntry
    {
        /// <summary>
        /// Время, когда сообщение было написано.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Класс игрока во время написания сообщения.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Уровень игрока.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Страна игрока.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// STEAM ID игрока.
        /// </summary>
        public string SteamId { get; set; }
    }
}
