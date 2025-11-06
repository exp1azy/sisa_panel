namespace Sisa.Panel.Models.Clans
{
    /// <summary>
    /// Запись с информацией о клане.
    /// </summary>
    public class ClanEntry
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Рейтинг клана.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Входит ли клан в топ.
        /// </summary>
        public bool InTop { get; set; }

        /// <summary>
        /// Название клана.
        /// </summary>
        public string ClanName { get; set; }

        /// <summary>
        /// Действия клана.
        /// </summary>
        public IReadOnlyList<string> Actions { get; set; }

        /// <summary>
        /// Количество игроков.
        /// </summary>
        public int PlayersCount { get; set; }
    }
}
