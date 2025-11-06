namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Информация о статистике использований игроком зомби-гранат.
    /// </summary>
    public class PlayerZombieGrenadesInfo
    {
        /// <summary>
        /// Название гранаты.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Нанесенный урон.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Количество заражений.
        /// </summary>
        public int? Infects { get; set; }

        /// <summary>
        /// Количество бросков.
        /// </summary>
        public int? Throws { get; set; }

        /// <summary>
        /// Количество убийств.
        /// </summary>
        public int Kills { get; set; }
    }
}
