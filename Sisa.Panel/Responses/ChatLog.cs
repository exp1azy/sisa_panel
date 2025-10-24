using Sisa.Panel.Models;

namespace Sisa.Panel.Responses
{
    public class ChatLog
    {
        public ICollection<ChatLogEntry> Messages { get; set; }
    }
}
