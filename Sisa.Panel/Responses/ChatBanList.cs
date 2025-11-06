using Sisa.Panel.Models.ChatBanList;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Список банов чата и микрофона.
    /// </summary>
    public class ChatBanList
    {
        /// <summary>
        /// Список банов.
        /// </summary>
        public IReadOnlyList<ChatBanEntry> ChatBans { get; set; }

        /// <summary>
        /// Общее количество банов.
        /// </summary>
        public int TotalBans { get; set; }

        /// <summary>
        /// Количество активных банов.
        /// </summary>
        public int ActiveBans { get; set; }
    }
}
