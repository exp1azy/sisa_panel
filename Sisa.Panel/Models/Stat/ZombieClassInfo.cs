namespace Sisa.Panel.Models.Stat
{
    public class ZombieClassInfo
    {
        public string Name { get; set; }

        public int? Health { get; set; }

        public int? Speed { get; set; }

        public string? Knockback { get; set; }

        public int? Infects { get; set; }

        public int HumansKills { get; set; }

        public int SurvivorsKills { get; set; }

        public int Deaths { get; set; }

        public int Games { get; set; }

        public long Damage { get; set; }

        public int? FirstZm { get; set; }

        public int Suicides { get; set; }

        public ZombieTopPlayerEntry[] Players { get; set; } = new ZombieTopPlayerEntry[10];
    }
}
