namespace Sisa.Panel.Models.Clans
{
    /// <summary>
    /// Запись с информацией о последнем действии в клане.
    /// </summary>
    public class ClanLastActionEntry
    {
        /// <summary>
        /// Вид действия.
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// Действие.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Игрок, выполнивший действие.
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// Дата совершения действия.
        /// </summary>
        public DateOnly Date { get; set; }
    }
}
