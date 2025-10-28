namespace Sisa.Panel.Models.Live
{
    public class ServerStatistics
    {
        public Dictionary<string, int> HourlyActivity { get; set; }

        public Dictionary<string, int> MonthlyActivity { get; set; }
    }
}
