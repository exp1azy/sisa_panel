namespace Sisa.Panel.Models
{
    public abstract class BaseClassInfo
    {
        public string Name { get; set; }

        public int Gravity { get; set; }

        public int ZombieKills { get; set; }

        public int NemesisKills { get; set; }

        public int Deaths { get; set; }

        public int Suicides { get; set; }

        public long Damage { get; set; }
    }
}
