namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Информация о временном оружии игрока.
    /// </summary>
    public class PlayerTempWeapon
    {
        /// <summary>
        /// Является ли основным.
        /// </summary>
        public bool Main { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Оставшееся время.
        /// </summary>
        public string RemainingTime { get; set; }
    }
}
