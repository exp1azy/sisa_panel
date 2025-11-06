namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Общая информация об игроке.
    /// </summary>
    public class PlayerGeneralInfo
    {
        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тег клана.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Страна.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Ссылка на профиль Steam.
        /// </summary>
        public string SteamProfileUrl { get; set; }

        /// <summary>
        /// Имя профиля Steam.
        /// </summary>
        public string SteamProfileName { get; set; }

        /// <summary>
        /// STEAM ID игрока.
        /// </summary>
        public string SteamId { get; set; }

        /// <summary>
        /// Дата последнего посещения сервера.
        /// </summary>
        public DateTime LastVisitedAt { get; set; }

        /// <summary>
        /// Уровень игрока.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Состоит в клане.
        /// </summary>
        public string ClanMember { get; set; }

        /// <summary>
        /// Онлайн.
        /// </summary>
        public string Online { get; set; }
    }
}
