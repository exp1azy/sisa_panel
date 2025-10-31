using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    public class PlayerInfo
    {
        public PlayerGeneralInfo GeneralInfo { get; set; }

        public PlayerSettings Settings { get; set; }

        public PlayerBasicStats BasicStats { get; set; }

        public PlayerProgress Progress { get; set; }

        public IReadOnlyList<PlayerTempWeapon> TempWeapons { get; set; }

        public IReadOnlyList<PlayerWeaponStatEntry> WeaponStats { get; set; }

        public IReadOnlyList<PlayerModWeaponStatEntry> ModWeaponStats { get; set; }

        public IReadOnlyList<PlayerZombieStatEntry> ZombieClassesStats { get; set; }

        public IReadOnlyList<PlayerZombieGrenadesInfo> ZombieGrenades { get; set; }
    }
}
