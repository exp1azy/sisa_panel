namespace Sisa.Panel.Models.Live
{
    /// <summary>
    /// Статус сервера.
    /// </summary>
    public class ServerStatus
    {
        /// <summary>
        /// Текущая карта.
        /// </summary>
        public string CurrentMap { get; set; }

        /// <summary>
        /// Текущий режим.
        /// </summary>
        public string CurrentMod { get; set; }

        /// <summary>
        /// Игроков онлайн.
        /// </summary>
        public int PlayersOnline { get; set; }

        /// <summary>
        /// Максимальное количество игроков.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Оставшееся время на текущей карте.
        /// </summary>
        public string TimeLeft { get; set; }

        /// <summary>
        /// Доступен ли босс-раунд.
        /// </summary>
        public bool BossRoundAvailable { get; set; }

        /// <summary>
        /// Ссылка на изображение карты.
        /// </summary>
        public string MapImage { get; set; }
    }
}
