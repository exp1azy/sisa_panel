namespace Sisa.Panel.Models.Stat
{
    public class HumanClassInfo
    {
        public string Name { get; set; }

        public int Gravity { get; set; }

        public int ZombieKills { get; set; }

        public int NemesisKills { get; set; }

        public int Deaths { get; set; }

        public int Suicides { get; set; }

        public long Damage { get; set; }

        public int? Health { get; set; }

        public int? Speed { get; set; }

        public int? Games { get; set; }

        public int? WasInfected { get; set; }

        public HumanTopPlayerEntry[] Players { get; set; } = new HumanTopPlayerEntry[10];
    }
}
