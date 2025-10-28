namespace Sisa.Panel.Models.Clans
{
    public class ClanGeneralInfo
    {
        public ICollection<string> Actions { get; set; }

        public int Online { get; set; }

        public int Bank { get; set; }

        public int Ammo { get; set; }

        public int ZombieKills { get; set; }

        public int NemesisKills { get; set; }

        public int BossKills { get; set; }

        public int HumanKills { get; set; }

        public int SurvivorKills { get; set; }

        public int Infections { get; set; }
    }
}
