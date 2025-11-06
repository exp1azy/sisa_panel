namespace Sisa.Panel.Models.Live
{
    /// <summary>
    /// Статистика сервера.
    /// </summary>
    public class ServerStatistics
    {
        /// <summary>
        /// Активность игроков за последний час.
        /// </summary>
        public IDictionary<string, int> HourlyActivity { get; set; }

        /// <summary>
        /// Средний онлайн за месяц.
        /// </summary>
        public IDictionary<string, int> MonthlyActivity { get; set; }
    }
}
