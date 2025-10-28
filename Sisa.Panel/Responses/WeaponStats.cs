using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    public class WeaponStats
    {
        public IList<WeaponEntry> Weapons { get; set; }

        public IList<ModWeaponEntry> ModWeapons { get; set; }
    }
}
