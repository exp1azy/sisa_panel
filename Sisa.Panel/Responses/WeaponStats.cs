using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Статистика по оружию.
    /// </summary>
    public class WeaponStats
    {
        /// <summary>
        /// Список пушек.
        /// </summary>
        public IReadOnlyList<WeaponEntry> Weapons { get; set; }

        /// <summary>
        /// Список модового оружия.
        /// </summary>
        public IReadOnlyList<ModWeaponEntry> ModWeapons { get; set; }
    }
}
