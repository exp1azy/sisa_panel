namespace Sisa.Panel.Models.Stat
{
    /// <summary>
    /// Настройки игрока.
    /// </summary>
    public class PlayerSettings
    {
        /// <summary>
        /// Включена ли запись демо.
        /// </summary>
        public bool RecordingDemo { get; set; }

        /// <summary>
        /// Включен ли режим героя.
        /// </summary>
        public bool Hero { get; set; }

        /// <summary>
        /// Включен ли режим героини.
        /// </summary>
        public bool Heroine { get; set; }

        /// <summary>
        /// Класс по умолчанию.
        /// </summary>
        public string DefaultClass { get; set; }

        /// <summary>
        /// Язык.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Включены ли туториалы.
        /// </summary>
        public bool Tutorials { get; set; }
    }
}
