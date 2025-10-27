namespace Sisa.Panel.Models
{
    public class ClanPlayerEntry
    {
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsModerator { get; set; }

        public int Level { get; set; }

        public string Online { get; set; }

        public DateTime LastActivity { get; set; }
    }
}
