namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Достижения игрока.
    /// </summary>
    public class PlayerProgress
    {
        /// <summary>
        /// Класс зомби.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Нанес урона за зомби.
        /// </summary>
        public int DamageByZombie { get; set; }

        /// <summary>
        /// Заразил.
        /// </summary>
        public int Infects { get; set; }

        /// <summary>
        /// Был заражен.
        /// </summary>
        public int WasInfected { get; set; }

        /// <summary>
        /// Был первым зомби.
        /// </summary>
        public int WasFirstZm { get; set; }

        /// <summary>
        /// Был немезисом.
        /// </summary>
        public int WasNemesis { get; set; }

        /// <summary>
        /// Был выжившим.
        /// </summary>
        public int WasSurvivor { get; set; }

        /// <summary>
        /// Был героем.
        /// </summary>
        public int WasHero { get; set; }

        /// <summary>
        /// Был героиней.
        /// </summary>
        public int WasHeroine { get; set; }

        /// <summary>
        /// Убил зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Убил людей.
        /// </summary>
        public int HumanKills { get; set; }

        /// <summary>
        /// Убил немезисов.
        /// </summary>
        public int NemesisKills { get; set; }

        /// <summary>
        /// Убил выживших.
        /// </summary>
        public int SurvivorKills { get; set; }

        /// <summary>
        /// Убил боссов.
        /// </summary>
        public int BossKills { get; set; }
    }
}
