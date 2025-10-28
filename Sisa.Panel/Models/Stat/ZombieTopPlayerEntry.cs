namespace Sisa.Panel.Models.Stat
{
    public class ZombieTopPlayerEntry : TopPlayerEntry
    {
        public int? Infects { get; set; }

        public int HumanKills { get; set; }

        public int SurvivorKills { get; set; }
    }
}
