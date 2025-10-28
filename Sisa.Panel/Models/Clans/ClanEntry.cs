namespace Sisa.Panel.Models.Clans
{
    public class ClanEntry
    {
        public int Id { get; set; }

        public float Rating { get; set; }

        public bool InTop { get; set; }

        public string ClanName { get; set; }

        public ICollection<string> Actions { get; set; }

        public int PlayersCount { get; set; }
    }
}
