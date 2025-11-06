using Sisa.Panel.Models.Stat;

namespace Sisa.Panel.Responses
{
    /// <summary>
    /// Информация об игроке.
    /// </summary>
    public class PlayerInfo
    {
        /// <summary>
        /// Общая информация.
        /// </summary>
        public PlayerGeneralInfo GeneralInfo { get; set; }

        /// <summary>
        /// Настройки.
        /// </summary>
        public PlayerSettings Settings { get; set; }

        /// <summary>
        /// Основная статистика.
        /// </summary>
        public PlayerBasicStats BasicStats { get; set; }

        /// <summary>
        /// Достижения.
        /// </summary>
        public PlayerProgress Progress { get; set; }

        /// <summary>
        /// Оружие на время.
        /// </summary>
        public IReadOnlyList<PlayerTempWeapon> TempWeapons { get; set; }

        /// <summary>
        /// Статистика по оружию.
        /// </summary>
        public IReadOnlyList<PlayerWeaponStatEntry> WeaponStats { get; set; }

        /// <summary>
        /// Статистика по модовому оружию.
        /// </summary>
        public IReadOnlyList<PlayerModWeaponStatEntry> ModWeaponStats { get; set; }

        /// <summary>
        /// Статистика по классам зомби.
        /// </summary>
        public IReadOnlyList<PlayerZombieStatEntry> ZombieClassesStats { get; set; }

        /// <summary>
        /// Статистика по гранатам зомби.
        /// </summary>
        public IReadOnlyList<PlayerZombieGrenadesInfo> ZombieGrenades { get; set; }
    }
}
