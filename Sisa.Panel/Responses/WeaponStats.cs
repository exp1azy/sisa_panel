using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class WeaponStats
    {
        public List<WeaponEntry> Weapons { get; set; }

        public List<ModWeaponEntry> ModWeapons { get; set; }
    }
}
