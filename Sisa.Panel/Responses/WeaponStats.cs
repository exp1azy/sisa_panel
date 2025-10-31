using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    public class WeaponStats
    {
        public IReadOnlyList<WeaponEntry> Weapons { get; set; }

        public IReadOnlyList<ModWeaponEntry> ModWeapons { get; set; }
    }
}
