namespace Sisa.Panel.Models
{
    public class ServerStatistics
    {
        public IDictionary<string, int> HourlyActivity { get; set; }

        public IDictionary<string, int> MonthlyActivity { get; set; }
    }
}
