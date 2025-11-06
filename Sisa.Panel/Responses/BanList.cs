using Sisa.Panel.Models.BanList;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Список банов с общей статистикой.
    /// </summary>
    public class BanList
    {
        /// <summary>
        /// Список банов.
        /// </summary>
        public IReadOnlyList<BanEntry> Bans { get; set; }

        /// <summary>
        /// Общее количество банов.
        /// </summary>
        public int TotalBans { get; set; }

        /// <summary>
        /// Количество активных банов.
        /// </summary>
        public int ActiveBans { get; set; }

        /// <summary>
        /// Всего демо.
        /// </summary>
        public int TotalDemos { get; set; }
    }
}
