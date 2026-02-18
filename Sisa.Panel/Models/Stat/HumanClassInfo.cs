namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Информация о классе человека.
    /// </summary>
    public class HumanClassInfo
    {
        /// <summary>
        /// Название класса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Гравитация.
        /// </summary>
        public int Gravity { get; set; }

        /// <summary>
        /// Количество убийств зомби.
        /// </summary>
        public int ZombieKills { get; set; }

        /// <summary>
        /// Количество убийств немезиса.
        /// </summary>
        public int NemesisKills { get; set; }

        /// <summary>
        /// Количество смертей.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// Количество самоубийств.
        /// </summary>
        public int Suicides { get; set; }

        /// <summary>
        /// Урон.
        /// </summary>
        public long Damage { get; set; }

        /// <summary>
        /// Здоровье.
        /// </summary>
        public int? Health { get; set; }

        /// <summary>
        /// Скорость.
        /// </summary>
        public int? Speed { get; set; }

        /// <summary>
        /// Количество игр.
        /// </summary>
        public int? Games { get; set; }

        /// <summary>
        /// Был заражен.
        /// </summary>
        public int? WasInfected { get; set; }

        /// <summary>
        /// Ссылка на изображение класса.
        /// </summary>
        public string ClassImage { get; set; }

        /// <summary>
        /// Топ-10 лучших игроков за людей.
        /// </summary>
        public HumanTopPlayerEntry[] Players { get; set; } = new HumanTopPlayerEntry[10];
    }
}
