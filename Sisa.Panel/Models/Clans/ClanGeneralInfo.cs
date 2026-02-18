namespace Sisa.Panel.Models.Clans
{
    /// <summary>
    /// Общая информация о клане.
    /// </summary>
    public class ClanGeneralInfo
    {
        /// <summary>
        /// Действия клана.
        /// </summary>
        public IReadOnlyList<string> Actions { get; set; }

        /// <summary>
        /// Общий онлайн (в днях).
        /// </summary>
        public int Online { get; set; }

        /// <summary>
        /// Банк клана.
        /// </summary>
        public int Bank { get; set; }

        /// <summary>
        /// Общее количество аммо клана.
        /// </summary>
        public int Ammo { get; set; }
        
        /// <summary>
        /// Количество убитых зомби участниками клана.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Количество убитых немезисов участниками клана.
        /// </summary>
        public int NemesisKills { get; set; }

        /// <summary>
        /// Количество убитых боссов участниками клана.
        /// </summary>
        public int BossKills { get; set; }

        /// <summary>
        /// Количество убитых людей участниками клана.
        /// </summary>
        public int HumanKills { get; set; }

        /// <summary>
        /// Количество убитых выживших участниками клана.
        /// </summary>
        public int SurvivorKills { get; set; }

        /// <summary>
        /// Количество заражений участниками клана.
        /// </summary>
        public int Infections { get; set; }

        /// <summary>
        /// Ссылка на изображение клана.
        /// </summary>
        public string ClanImage { get; set; }
    }
}
