namespace Sisa.Panel.Models.Stat
{
    public class PlayerProgress
    {
        public string Class { get; set; }

        public int DamageByZombie { get; set; }

        public int Infects { get; set; }

        public int WasInfected { get; set; }

        public int WasFirstZm { get; set; }

        public int WasNemesis { get; set; }

        public int WasSurvivor { get; set; }

        public int WasHero { get; set; }

        public int WasHeroine { get; set; }

        public int ZombieKills { get; set; }

        public int HumanKills { get; set; }

        public int NemesisKills { get; set; }

        public int SurvivorKills { get; set; }

        public int BossKills { get; set; }
    }
}
