using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class ChatBanList
    {
        public ICollection<ChatBanEntry> ChatBans { get; set; }

        public int TotalBans { get; set; }

        public int ActiveBans { get; set; }
    }
}
