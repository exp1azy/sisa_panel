namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Основная статистика игрока.
    /// </summary>
    public class PlayerBasicStats
    {
        /// <summary>
        /// Нож.
        /// </summary>
        public string Knife { get; set; }

        /// <summary>
        /// Опыт.
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Следующий уровень.
        /// </summary>
        public int NextLevel { get; set; }

        /// <summary>
        /// Опыт до следующего уровня.
        /// </summary>
        public int ExpToNextLevel { get; set; }

        /// <summary>
        /// Деньги.
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// Аммо.
        /// </summary>
        public int Ammo { get; set; }

        /// <summary>
        /// Денежные ключи.
        /// </summary>
        public int MoneyKeys { get; set; }

        /// <summary>
        /// Аммо ключи.
        /// </summary>
        public int AmmoKeys { get; set; }

        /// <summary>
        /// Нанес урона.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Был лучшим.
        /// </summary>
        public int MVPs { get; set; }

        /// <summary>
        /// Ассистов.
        /// </summary>
        public int Assists { get; set; }

        /// <summary>
        /// Самоубийств.
        /// </summary>
        public int Suicides { get; set; }

        /// <summary>
        /// Смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Соотношение убийств к смертям.
        /// </summary>
        public decimal KillDeathRatio { get; set; }
    }
}
