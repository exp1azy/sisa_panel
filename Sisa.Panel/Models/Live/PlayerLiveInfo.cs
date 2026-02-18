namespace Sisa.Panel.Models.Live
{
    /// <summary>
    /// Информация об игроке, присутствующем на сервере.
    /// </summary>
    public class PlayerLiveInfo
    {
        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// STEAM ID игрока.
        /// </summary>
        public string SteamId { get; set; }

        /// <summary>
        /// Страна.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Количество убийств.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Количество смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Время игры.
        /// </summary>
        public string PlayTime { get; set; }

        /// <summary>
        /// Пинг игрока.
        /// </summary>
        public int Ping { get; set; }

        /// <summary>
        /// Уровень игрока.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Команда, за которую играет (зомби или люди).
        /// </summary>
        public PlayerTeam Team { get; set; }

        /// <summary>
        /// Ссылка на фотографию игрока.
        /// </summary>
        public string Image { get; set; }
    }
}
