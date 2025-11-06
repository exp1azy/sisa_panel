using Sisa.Panel.Models.Clans;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Информация о клане.
    /// </summary>
    public class ClanInfo
    {
        /// <summary>
        /// Общая информация о клане.
        /// </summary>
        public ClanGeneralInfo GeneralInfo { get; set; }

        /// <summary>
        /// Последние действия в клане.
        /// </summary>
        public IReadOnlyList<ClanLastActionEntry> LastActions { get; set; }

        /// <summary>
        /// Участники клана.
        /// </summary>
        public IReadOnlyList<ClanPlayerEntry> Members { get; set; }
    }
}
