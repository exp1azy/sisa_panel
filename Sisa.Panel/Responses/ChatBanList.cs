using Sisa.Panel.Models.ChatBanList;

namespace Sisa.Panel.Responses
{
    public class ChatBanList
    {
        public IList<ChatBanEntry> ChatBans { get; set; }

        public int TotalBans { get; set; }

        public int ActiveBans { get; set; }
    }
}
