namespace Sisa.Panel.Models.Live
{
    /// <summary>
    /// Информация о последних сыгранных картах.
    /// </summary>
    public class LivePreviousMap
    {
        /// <summary>
        /// Название карты.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на изображение карты.
        /// </summary>
        public string MapImage { get; set; }
    }
}
