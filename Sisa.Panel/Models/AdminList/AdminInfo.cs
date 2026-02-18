namespace Sisa.Panel.Models.AdminList
{
    /// <summary>
    /// Информация об админе.
    /// </summary>
    public class AdminInfo
    {
        /// <summary>
        /// Имя админа.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Steam профиль.
        /// </summary>
        public string SteamProfile { get; set; }

        /// <summary>
        /// Ссылка на фотографию админа.
        /// </summary>
        public string Image { get; set; }
    }
}
